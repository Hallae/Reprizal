using myApi.Data;
using myApi.Interfaces;

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
    }
}
