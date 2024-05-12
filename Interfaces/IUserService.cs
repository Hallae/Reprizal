namespace myApi.Services.UserService
{
    public interface IUserService
    {
        /// <summary>
        /// For unit test "register" to check and mock
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        Task CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string GetMyName();
        Task<User> Register(UserDto userDto);
        Task<string> Login(UserDto userDto);
    }
}
