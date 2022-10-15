using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace patchawallet.holiday.api.unittest
{
    public class HolidayControllerFixture : FixtureBase
    {
        [Fact]
        public void GetHolidaysPaginatedShould()
        {
            // Arrange
            var holidays = LoadJson<List<Holiday>>("holidays").Take(10).ToVM();
            var serviceMock = new Mock<IHolidayService>();
            serviceMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(holidays);

            // Action
            var controller = new HolidaysController(serviceMock.Object);
            var result = controller.Get(1, 10, string.Empty).Result;
            var okResult = result as OkObjectResult;
            var value = (List<HolidayVM>)okResult.Value;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(value.Count > 9);
        }

        [Fact]
        public void GetHolidayByIdShould()
        {
            // Arrange
            var holiday = LoadJson<List<Holiday>>("holidays").FirstOrDefault(x => x.Id == 10).ToVM();
            var serviceMock = new Mock<IHolidayService>();
            serviceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(holiday);

            // Action
            var controller = new HolidaysController(serviceMock.Object);
            var result = controller.Get(10).Result;
            var okResult = result as OkObjectResult;
            var value = (HolidayVM)okResult.Value;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(10, value.Id);
        }

        [Fact]
        public void PostHolidayShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var vm = request.ToEntity().ToVM();
            var serviceMock = new Mock<IHolidayService>();
            serviceMock.Setup(x => x.PostAsync(request)).ReturnsAsync(vm);

            // Action
            var controller = new HolidaysController(serviceMock.Object);
            var result = controller.Post(request).Result;
            var okResult = result as OkObjectResult;
            var value = (HolidayVM)okResult.Value;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("New Holiday", value.Description);
        }

        [Fact]
        public void PutHolidayShould()
        {
            // Arrange
            var request = new HolidayInserOrUpdateVM() { Description = "New Holiday", Date = DateTime.Now, Type = "interno" };
            var vm = request.ToEntity().ToVM();
            var serviceMock = new Mock<IHolidayService>();
            serviceMock.Setup(x => x.UpdateAsync(request)).ReturnsAsync(vm);

            // Action
            var controller = new HolidaysController(serviceMock.Object);
            var result = controller.Put(request).Result;
            var okResult = result as OkObjectResult;
            var value = (HolidayVM)okResult.Value;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("New Holiday", value.Description);
        }

        [Fact]
        public void DeleteHolidayShould()
        {
            // Arrange
            var serviceMock = new Mock<IHolidayService>();
            serviceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Action
            var controller = new HolidaysController(serviceMock.Object);
            var result = controller.Delete(10).Result;
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
