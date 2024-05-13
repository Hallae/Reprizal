using myApi.Data;
using myApi.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace myApi.Repository
{
    public class ApplicationRepository : IContextApplication
    {
        private readonly DataContext _context;
    
        public ApplicationRepository(DataContext context)
        {
         
            _context = context;
        }
        public Task<List<Application>> GetAllAsync()
        {
            return _context.Application.ToListAsync();

        }

       
        public async Task<IEnumerable<Activity>> GetActivitiesAsync()
        {
            var activities = new List<Activity>
            {
        new Activity { ActivityType = "Report", Description = "Доклад, 35-45 минут" },
        new Activity { ActivityType = "Masterclass", Description = "Мастеркласс, 1-2 часа" },
        new Activity { ActivityType = "Discussion", Description = "Дискуссия / круглый стол, 40-50 минут" }
            };

            return activities;
        }

        public async Task<Application> FindAsync(Guid id)
        {
            return await _context.Application.FindAsync(id);
        }

        public async Task UpdateApplication(Application application)
        {
            var dbApplication = await FindAsync(application.id);
            if (dbApplication == null)
                throw new Exception("Application not found");

            if (dbApplication.IsSubmitted)
                throw new Exception("Application has already been submitted and cannot be edited.");

            dbApplication.activity = application.activity;
            dbApplication.name = application.name;
            dbApplication.description = application.description;
            dbApplication.outline = application.outline;

            await _context.SaveChangesAsync();
        }
    }
}
