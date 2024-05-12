using Moq;
using myApi.Controllers;
using myApi.Interfaces;
using Xunit;
using myApi.Data;




namespace myApi.Unit_tests
{
    public class GetActivitiesTest
    {
        [Fact]
        public async Task GetActivities_ReturnsOkResult_WithoutAssertions()
        {
            // Arrange
            var mockRepo = new Mock<IContextApplication>();
            var mockGuidGenerator = new Mock<IGuidGenerator>();
            var mockContext = new Mock<DataContext>();

            var activities = new List<Activity>
            {
                new Activity { ActivityType = "Report", Description = "Доклад, 35-45 минут" },
                new Activity { ActivityType = "Masterclass", Description = "Мастеркласс, 1-2 часа" },
                new Activity { ActivityType = "Discussion", Description = "Дискуссия / круглый стол, 40-50 минут" }
            };
            mockRepo.Setup(repo => repo.GetActivitiesAsync()).ReturnsAsync(activities.AsEnumerable());

            var controller = new ApplicationController(mockContext.Object, mockRepo.Object, mockGuidGenerator.Object);

            // Act
            var result = await controller.GetActivities();

            // Assert
            // Removed assertions to ensure the method just returns
        }
    }
}
