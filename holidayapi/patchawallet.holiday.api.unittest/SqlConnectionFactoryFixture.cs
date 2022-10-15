using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace patchawallet.holiday.api.unittest
{
    public class SqlConnectionFactoryFixture : FixtureBase
    {
        [Fact]
        public void CreateConnectionShould()
        {
            // Arrange
            var options = new Mock<IOptions<SqlConnectionSettings>>();
            options.Setup(x => x.Value).Returns(new SqlConnectionSettings() { ConnectionString = "Server=test;Initial Catalog=patchawallet;Persist Security Info=False;User ID=sa;Password=DFkl1819$#@!;Connection Timeout=30" });
            var connectionMock = new Mock<IDbConnectionFactory>();
            var dbConnectionMock = new Mock<IDbConnection>();
            connectionMock.Setup(x => x.CreateConnection()).Returns(dbConnectionMock.Object);

            // Action
            var connection = new SqlConnectionFactory(options.Object);
            var result = connection.CreateConnection();

            // Assert
            Assert.NotNull(result);
        }

    }
}
