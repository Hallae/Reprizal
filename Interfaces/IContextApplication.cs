namespace myApi.Interfaces
{
    public interface IContextApplication
    {
        Task<List<Application>> GetAllAsync();
        Task<IEnumerable<Activity>> GetActivitiesAsync();
        Task<Application> FindAsync(Guid id);
    }
}
