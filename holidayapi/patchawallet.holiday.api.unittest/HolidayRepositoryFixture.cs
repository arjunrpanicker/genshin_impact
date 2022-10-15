using Dapper;
using Moq;
using Moq.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Xunit;

namespace patchawallet.holiday.api.unittest
{
    public class HolidayRepositoryFixture : FixtureBase
    {
        [Fact]
        public void AddHolidayShould()
        {
            // Arrange
            var holidays = LoadJson<List<Holiday>>("holidays");
            var finados = holidays.FirstOrDefault(x => x.Id == 10);
            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            var db = new Mock<DbConnection>();
            db.SetupDapperAsync(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<Holiday>(), It.IsAny<DbTransaction>(), null, null)).ReturnsAsync(1);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(db.Object);
            var repository = new HolidayRepository(connectionFactoryMock.Object);

            // Action
            var result = repository.AddAsync(finados).Result;

            // Assert
            Assert.Equal(10, result.Id);
            Assert.Equal("Finados (feriado nacional)", result.Description);
            Assert.Equal("nacional", result.Type);
        }

        [Fact]
        public void UpdateHolidayShould()
        {
            // Arrange
            var holidays = LoadJson<List<Holiday>>("holidays");
            var finados = holidays.FirstOrDefault(x => x.Id == 10);
            finados.Type = "municipal";
            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            var db = new Mock<DbConnection>();
            db.SetupDapperAsync(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<Holiday>(), It.IsAny<DbTransaction>(), null, null)).ReturnsAsync(1);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(db.Object);
            var repository = new HolidayRepository(connectionFactoryMock.Object);

            // Action
            var result = repository.UpdateAsync(finados).Result;

            // Assert
            Assert.Equal(10, result.Id);
            Assert.Equal("Finados (feriado nacional)", result.Description);
            Assert.Equal("municipal", result.Type);
        }

        [Fact]
        public void DeleteHolidayShould()
        {
            // Arrange
            var holidays = LoadJson<List<Holiday>>("holidays");
            var finados = holidays.FirstOrDefault(x => x.Id == 10);
            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            var db = new Mock<DbConnection>();
            db.SetupDapperAsync(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<Holiday>(), It.IsAny<DbTransaction>(), null, null)).ReturnsAsync(1);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(db.Object);
            var repository = new HolidayRepository(connectionFactoryMock.Object);

            // Action
            var result = repository.DeleteAsync(finados.Id).Result;

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetAllHolidaysShould()
        {
            // Arrange
            var holidays = LoadJson<List<Holiday>>("holidays");
            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            var db = new Mock<DbConnection>();
            db.SetupDapperAsync(x => x.QueryAsync<Holiday>(It.IsAny<string>(), It.IsAny<Holiday>(), It.IsAny<DbTransaction>(), null, null)).ReturnsAsync(holidays.Take(10));
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(db.Object);
            var repository = new HolidayRepository(connectionFactoryMock.Object);

            // Action
            var result = repository.GetAllAsync(1, 10, "").Result;

            // Assert
            Assert.Equal(10, result.Count());
        }

        [Fact]
        public void GetHolidayShould()
        {
            // Arrange
            var holidays = LoadJson<List<Holiday>>("holidays");
            var finados = holidays.FirstOrDefault(x => x.Id == 10);
            var connectionFactoryMock = new Mock<IDbConnectionFactory>();
            var db = new Mock<DbConnection>();
            db.SetupDapperAsync(x => x.QueryFirstOrDefaultAsync<Holiday>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DbTransaction>(), null, null)).ReturnsAsync(finados);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(db.Object);
            var repository = new HolidayRepository(connectionFactoryMock.Object);

            // Action
            var result = repository.GetAsync(10).Result;
            
            // Assert
            Assert.Equal(10, result.Id);
            Assert.Equal("Finados (feriado nacional)", result.Description);
            Assert.Equal("nacional", result.Type);
        }
    }
}
