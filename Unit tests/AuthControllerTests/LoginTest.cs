using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using myApi.Controllers; 
using myApi.Services.UserService; 

namespace myApi.Tests
{
    public class LoginTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IUserService> _mockUserService;
        private readonly AuthController _authController;

        public LoginTests()
        {
            // Initialize mocks
            _mockConfiguration = new Mock<IConfiguration>();
            _mockUserService = new Mock<IUserService>();

            // Initialize the AuthController with mocked dependencies
            _authController = new AuthController(_mockConfiguration.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsBadRequestResult()
        {
            // Arrange
            var userDto = new UserDto { Username = "invalidUser", Password = "wrongPassword" };

            // Act
            var result = await _authController.Login(userDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult.Value); // Assuming the error message or status code is not null
        }

        // Add more test methods as needed for latr tests
    }
}
