using Microsoft.AspNetCore.Mvc;
using myApi.Data;
using CsvHelper;
using System.Globalization;
using myApi.Interfaces;
using Microsoft.AspNetCore.Authorization;



namespace myApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]

    public class ApplicationController : ControllerBase
    {


        private readonly IGuidGenerator _guidGenerator;
        private readonly IContextApplication _contextrepo; 
        private readonly DataContext _context;
        
        public ApplicationController(DataContext context, IContextApplication contextrepo, IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _contextrepo = contextrepo;
            _context = context;
        }

        /// <summary>
        /// Get a list of all applications in the database
        /// </summary>
     
        [HttpGet]
        public async Task<ActionResult<List<Application>>> Get()
        {

            return Ok(await _contextrepo.GetAllAsync());
        }

        /// <summary>
        /// create a new appliction
        /// </summary>
        /// <param name="_application"></param>
   

        [HttpPost("Add")]
        public async Task<ActionResult<List<Application>>> Add(Application application)
           
        {
            application.id = _guidGenerator.GenerateNewId();
            application.author = _guidGenerator.GenerateNewAuthor();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingApplication = await _context.Application.FindAsync(application.id);
            if (existingApplication != null )
            {
                application.IsSubmitted = existingApplication.IsSubmitted;
            }
            else
            {
                application.IsSubmitted = false;
            }


            _context.Application.Add(application);
            await _context.SaveChangesAsync();

            return Ok(await _contextrepo.GetAllAsync());
        }



        /// <summary>
        /// Shows a list of unsubmitted applications older than a specified date
        /// </summary>
       
        [HttpGet("unsubmittedOlder")]
        public async Task<ActionResult<List<Application>>> GetUnsubmittedOlder()
        {
            DateTime specificDate = new DateTime(2024, 1, 1, 23, 0, 0, 0);

            var applications = await _context.Application
                .Where(a => a.SubmissionDate <= specificDate && !a.IsSubmitted)
                .ToListAsync();

            return Ok(applications);
        }

        /// <summary>
        /// Shows a list of submitted applications after than a specified date
        /// </summary>
        [HttpGet("submittedAfter")]
        public async Task<ActionResult<List<Application>>> GetSubmission()
        {
            DateTime specificDate = new DateTime(2024, 1, 1, 23, 0, 0, 0, DateTimeKind.Utc);

            var applications = await _context.Application
                .Where(a => a.SubmissionDate > specificDate)
                .ToListAsync();

            return Ok(applications);
        }

        /// <summary>
        /// Shows a list of activies
        /// </summary>
       
        [HttpGet("Activites")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetActivities()
        {
            var activities = await _contextrepo.GetActivitiesAsync();
            return Ok(activities);

        }


        /// <summary>
        /// Updates applications with new info
        /// </summary>
        /// <param name="request"></param>


        [HttpPut("Update")]
        public async Task<ActionResult<List<Application>>> UpdateApplication(Application request)
        {
            try
            {
                await _contextrepo.UpdateApplication(request);
                return Ok(await _contextrepo.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes applications by id
        /// </summary>
        /// <param name="id"></param>


        [HttpDelete("Delete")]
        public async Task<ActionResult<List<Application>>> DeleteApplication(Guid id)
        {
            var dbNewApplications = await _contextrepo.FindAsync(id);
            if (dbNewApplications == null)
                return BadRequest("Application not found");

            var dbApplications = await _contextrepo.FindAsync(id);
            if (dbApplications.IsSubmitted)
                return BadRequest("Submitted applications cannot be deleted");

            _context.Application.Remove(dbNewApplications);
            await _context.SaveChangesAsync();

            return Ok(await _contextrepo.GetAllAsync());
        }


        /// <summary>
        /// submits an application
        /// </summary>
        /// <param name="id"></param>

        [HttpPost("submit")]
        public async Task<ActionResult<List<Application>>> Submit(Guid id)
        {
            var dbNewApplications = await _contextrepo.FindAsync(id);
            if (dbNewApplications == null)
                return BadRequest("Application not found");


            dbNewApplications.IsSubmitted = true; // Set the application as submitted

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Exports a list of all application from the database
        /// </summary>

        [HttpGet("Export CSV")]
   public async Task<IActionResult> ExportApplications()
   {
       var applications = await _contextrepo.GetAllAsync();

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
