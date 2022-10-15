using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace patchawallet.holiday.api
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IOptions<SqlConnectionSettings> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
