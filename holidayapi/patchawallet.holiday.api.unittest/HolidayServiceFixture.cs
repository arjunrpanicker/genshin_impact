using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace patchawallet.holiday.api.unittest
{
    public class HolidayServiceFixture : FixtureBase
    {
        [Fact]
        public void GetAllHolidaysShould()
        {
            // Arrange
            var holidays = LoadJson<List<Holiday>>("holidays").Take(10);
            var repositoryMock = new Mock<IHolidayRepository>();           
            repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(holidays);
            var eventBusMock = new Mock<IEventBus>();
            var loggerMock = new Mock<ILogger<HolidayService>>();

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            var result = service.GetAllAsync(1, 10, "").Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
        }

        [Fact]
        public void GetHolidayByIdShould()
        {
            // Arrange
            var holiday = LoadJson<List<Holiday>>("holidays").FirstOrDefault(x => x.Id == 10);
            var repositoryMock = new Mock<IHolidayRepository>();
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(holiday);
            var eventBusMock = new Mock<IEventBus>();
            var loggerMock = new Mock<ILogger<HolidayService>>();

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            var result = service.GetByIdAsync(10).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("Finados (feriado nacional)", result.Description);
            Assert.Equal("nacional", result.Type);
        }

        [Fact]
        public void AddHolidayWithErrorNotDetectedShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Description = "New Holiday", Date = DateTime.Now, Type = "interno" };       
            var repositoryMock = new Mock<IHolidayRepository>();
            var description = "A network-related or instance-specific error occurred while establishing a connection to SQL Server";
            var error = new PostHolidayException(description);
            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Holiday>())).Throws(error); var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            Func<Task> act = () => service.PostAsync(request);

            //Assert
            var exception = Assert.ThrowsAsync<PostHolidayException>(act).Result;
            Assert.Equal(description, exception.Message);
        }

        [Fact]
        public void AddHolidayDuplicatedShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Description = "Finados (feriado nacional)", Date = DateTime.Parse("2021-11-02T00:00:00"), Type = "nacional" };
            var holidays = LoadJson<List<Holiday>>("holidays");
            var repositoryMock = new Mock<IHolidayRepository>();
            repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(holidays);
            var description = "Holiday Finados (feriado nacional) has already been cadastred with date 02/11/2021 00:00:00";
            var error = new HolidayAlreadyCadastredException(description);
            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Holiday>())).Throws(error);
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            Func<Task> act = () => service.PostAsync(request);

            //Assert
            var exception = Assert.ThrowsAsync<HolidayAlreadyCadastredException>(act).Result;
            Assert.Equal(description, exception.Message);
        }

        [Fact]
        public void AddHolidayShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var entity = request.ToEntity();
            var repositoryMock = new Mock<IHolidayRepository>();
            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Holiday>())).ReturnsAsync(entity);
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            var result = service.PostAsync(request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Holiday", result.Description);
        }

        [Fact]
        public void UpdateHolidayInvalidIdShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Id = 0, Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var repositoryMock = new Mock<IHolidayRepository>();
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();
            var description = "Invalid value on field id 0";
            var error = new HolidayNotFoundException(description);
            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Holiday>())).Throws(error);

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            Func<Task> act = () => service.UpdateAsync(request);

            // Assert
            var exception = Assert.ThrowsAsync<HolidayNotFoundException>(act).Result;
            Assert.Equal(description, exception.Message);
        }

        [Fact]
        public void UpdateHolidayNotFoundShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Id = 1, Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var repositoryMock = new Mock<IHolidayRepository>();
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();
            var description = "Holiday requested was not found";
            var error = new HolidayNotFoundException(description);
            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Holiday>())).Throws(error);

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            Func<Task> act = () => service.UpdateAsync(request);

            // Assert
            var exception = Assert.ThrowsAsync<HolidayNotFoundException>(act).Result;
            Assert.Equal(description, exception.Message);
        }


        [Fact]
        public void UpdateHolidayErrorQueringDatabaseShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Id = 1, Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var entity = request.ToEntity();
            var repositoryMock = new Mock<IHolidayRepository>();
            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Holiday>())).ReturnsAsync(entity);
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();
            var description = "Holiday requested was not found";
            var error = new QueryHolidayException(description);
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).Throws(error);

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            Func<Task> act = () => service.UpdateAsync(request);

            // Assert
            var exception = Assert.ThrowsAsync<QueryHolidayException>(act).Result;
            Assert.Equal(description, exception.Message);
        }


        [Fact]
        public void UpdateHolidayErrorTryingToUpdateShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Id = 1, Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var entity = request.ToEntity();
            var repositoryMock = new Mock<IHolidayRepository>();
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(entity);
            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Holiday>())).ReturnsAsync(entity);
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();
            var description = "Holiday requested was not found";
            var error = new PutHolidayException(description);
            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Holiday>())).Throws(error);

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            Func<Task> act = () => service.UpdateAsync(request);

            // Assert
            var exception = Assert.ThrowsAsync<PutHolidayException>(act).Result;
            Assert.Equal(description, exception.Message);
        }

        [Fact]
        public void UpdateHolidayShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Id = 1, Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var entity = request.ToEntity();
            var repositoryMock = new Mock<IHolidayRepository>();
            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Holiday>())).ReturnsAsync(entity);
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(entity);
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            var result = service.UpdateAsync(request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Holiday", result.Description);
        }

        [Fact]
        public void DeleteHolidayErrorShould()
        {
            // Arrange
            var repositoryMock = new Mock<IHolidayRepository>();
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();
            var description = "Holiday requested error";
            var error = new DeleteHolidayException(description);
            repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).Throws(error);

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            Func<Task> act = () => service.DeleteAsync(10);

            // Assert
            var exception = Assert.ThrowsAsync<DeleteHolidayException>(act).Result;
            Assert.Equal(description, exception.Message);
        }

        [Fact]
        public void DeleteHolidayShould()
        {
            // Arrange
            var repositoryMock = new Mock<IHolidayRepository>();
            repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1);
            var eventBusMock = new Mock<IEventBus>();
            eventBusMock.Setup(x => x.Publish(It.IsAny<IntegrationEvent>()));
            var loggerMock = new Mock<ILogger<HolidayService>>();

            // Action
            var service = new HolidayService(repositoryMock.Object, eventBusMock.Object, loggerMock.Object);
            var result = service.DeleteAsync(10).Result;

            // Assert
            Assert.True(result);
        }
    }
}
