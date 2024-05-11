namespace myApi.Interfaces
{
    public interface IContextApplication
    {
        Task<List<Application>> GetAllAsync();
        Task<IEnumerable<Activity>> GetActivitiesAsync();
    }
}
