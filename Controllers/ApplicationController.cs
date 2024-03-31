﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myApi.Data;
using System.Diagnostics;


namespace myApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {


        //so changes can be updated directly from database
        private readonly DataContext _context;
        private readonly ActivityDbContext _activity;
        public ApplicationController(DataContext context , ActivityDbContext activityDbContext)
        {
            _activity = activityDbContext;
            _context = context;
        }
     

        [HttpGet]
        public async Task<ActionResult<List<Application>>> Get()
        {

            return Ok(await _context.Application.ToListAsync());
        }

        /* [HttpPost]
         public async Task<ActionResult<List<NewApplications>>> AddMember(NewApplications _application)
         {
             // add and then enables save changes to database.
             _context.NewApplication.Add(_application);
             await _context.SaveChangesAsync();

             return Ok(await _context.NewApplication.ToListAsync());
         }
        */
        [HttpPost("Add")]
        public async Task<ActionResult<List<Application>>> Add(Application _application)
        {
            var existingApplication = await _context.Application.FindAsync(_application.id);
            if (existingApplication != null)
            {
                _application.IsSubmitted = existingApplication.IsSubmitted;
            }
            else
            {
                _application.IsSubmitted = false;
            }

            _context.Application.Add(_application);
            await _context.SaveChangesAsync();

            return Ok(await _context.Application.ToListAsync());
        }



        [HttpGet("unsubmittedOlder")]
        public async Task<ActionResult<List<Application>>> GetUnsubmittedOlder()
        {
            DateTime specificDate = new DateTime(2024, 1, 1, 23, 0, 0, 0);

            var applications = await _context.Application
                .Where(a => a.SubmissionDate <= specificDate && !a.IsSubmitted)
                .ToListAsync();

            return Ok(applications);
        }

        [HttpGet("submittedAfter")]
        public async Task<ActionResult<List<Application>>> GetSubmission()
        {
            DateTime specificDate = new DateTime(2024, 1, 1, 23, 0, 0, 0);

            var applications = await _context.Application
                .Where(a => a.SubmissionDate > specificDate)
                .ToListAsync();

            return Ok(applications);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Activity>> GetActivities()
        {
            var activities = new List<Activity>
            {
                new Activity { ActivityType = "Report", Description = "Доклад, 35-45 минут" },
                new Activity { ActivityType = "Masterclass", Description = "Мастеркласс, 1-2 часа" },
                new Activity { ActivityType = "Discussion", Description = "Дискуссия / круглый стол, 40-50 минут" }
            };

            return activities;
        }



        [HttpPut("Update")]
        public async Task<ActionResult<List<Application>>> UpdateApplication(Application request)
        {
            // add and then enables save changes to database.
            var dbApplications = await _context.Application.FindAsync(request.id);
            if (dbApplications == null)
                return BadRequest("Application not found");

            dbApplications.activity = request.activity;
            dbApplications.name = request.name;
            dbApplications.description = request.description;
            dbApplications.outline = request.outline;


            await _context.SaveChangesAsync();

            return Ok(await _context.Application.ToListAsync());
        }


        [HttpDelete("Delete")]
        public async Task<ActionResult<List<Application>>> DeleteApplication(int id)
        {
            var dbNewApplications = await _context.Application.FindAsync(id);
            if (dbNewApplications == null)
                return BadRequest("Application not found");

            _context.Application.Remove(dbNewApplications);
            await _context.SaveChangesAsync();

            return Ok(await _context.Application.ToListAsync());
        }

        [HttpPost("submit")]
        public async Task<ActionResult<List<Application>>> Submit(int id)
        {
            var dbNewApplications = await _context.Application.FindAsync(id);
            if (dbNewApplications == null)
                return BadRequest("Application not found");


            dbNewApplications.IsSubmitted = true; // Set the application as submitted

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}