namespace myApi.Services.UserService
{
    public interface IUserService
    {
        Task CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string GetMyName();
    }
}
