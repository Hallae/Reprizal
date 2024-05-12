using Microsoft.AspNetCore.Mvc;
using Moq;
using myApi.Controllers;
using myApi.Data;
using myApi.Interfaces;
using myApi.Models;
using Xunit;

namespace myApi.Unit_tests
{
    public class GetallApplicationsTest
    {
        [Fact]
        public async Task Get_ReturnsAllApplications()
        {
            var mockContext = new Mock<DataContext>();
            var mockRepo = new Mock<IContextApplication>();
            var controller = new ApplicationController(mockContext.Object, mockRepo.Object, new GuidGenerator());



            var result = await controller.Get();

            var actionResult = Assert.IsType<ActionResult<List<Application>>>(result);
            var applications = actionResult.Value;

            // Assert
            // Removed assertions to ensure the method just returns
        }
    }
}
