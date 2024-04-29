using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myApi.Data;
using System.Diagnostics;
using CsvHelper;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;


namespace myApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class ApplicationController : ControllerBase
    {


        //so changes can be updated directly from database
        private readonly DataContext _context;
        private readonly ActivityDbContext _activity;
        public ApplicationController(DataContext context, ActivityDbContext activityDbContext)
        {
            _activity = activityDbContext;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Application>>> Get()
        {

            return Ok(await _context.Application.ToListAsync());
        }


        [HttpPost("Add")]
        public async Task<ActionResult<List<Application>>> Add(Application _application)
           
        {
            _application.id = Guid.NewGuid();
            _application.author = Guid.NewGuid();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        [HttpGet("Activities")]
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
            var dbApplications = await _context.Application.FindAsync(request.id);
            if (dbApplications == null)
                return BadRequest("Application not found");

            // Check if the application has been submitted
            if (dbApplications.IsSubmitted)
                return BadRequest("Application has already been submitted and cannot be edited.");

            dbApplications.activity = request.activity;
            dbApplications.name = request.name;
            dbApplications.description = request.description;
            dbApplications.outline = request.outline;

            await _context.SaveChangesAsync();

            return Ok(await _context.Application.ToListAsync());
        }



        [HttpDelete("Delete")]
        public async Task<ActionResult<List<Application>>> DeleteApplication(Guid id)
        {
            var dbNewApplications = await _context.Application.FindAsync(id);
            if (dbNewApplications == null)
                return BadRequest("Application not found");

            var dbApplications = await _context.Application.FindAsync(id);
            if (dbApplications.IsSubmitted)
                return BadRequest("Submitted applications cannot be deleted");

            _context.Application.Remove(dbNewApplications);
            await _context.SaveChangesAsync();

            return Ok(await _context.Application.ToListAsync());
        }

        [HttpPost("submit")]
        public async Task<ActionResult<List<Application>>> Submit(Guid id)
        {
            var dbNewApplications = await _context.Application.FindAsync(id);
            if (dbNewApplications == null)
                return BadRequest("Application not found");


            dbNewApplications.IsSubmitted = true; // Set the application as submitted

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("Export CSV")]
        public async Task<IActionResult> ExportApplications()
        {
            var applications = await _context.Application.ToListAsync();

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(applications);
                await streamWriter.FlushAsync();
                memoryStream.Position = 0;

                return File(memoryStream.ToArray(), "text/csv", "applications.csv");
            }
        }
       


    }
}