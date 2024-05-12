using Microsoft.AspNetCore.Mvc;
using Moq;
using myApi.Controllers;
using myApi.Services.UserService;
using Xunit;
using myApi.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace myApi.Unit_tests
{
    public class RegisterTest
    {
        [Fact]
        public async Task Register_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(service => service.CreatePasswordHash(It.IsAny<string>(), out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny))
                    .Returns(Task.CompletedTask);

            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            var controller = new AuthController(configuration, mockService.Object);

            var request = new UserDto
            {
                Username = "testUser",
                Password = "testPassword"
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var user = Assert.IsAssignableFrom<User>(okResult.Value);
            Assert.Equal(request.Username, user.Username);
            Assert.NotNull(user.PasswordHash);
            Assert.NotNull(user.PasswordSalt);
        }
    }
}
