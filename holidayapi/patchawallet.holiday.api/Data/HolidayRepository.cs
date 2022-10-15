using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public HolidayRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Holiday> AddAsync(Holiday holiday)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = @"INSERT INTO [dbo].[Holidays]
                                   ([Description]
                                   ,[Type]
                                   ,[Date]
                                   ,[Created]
                                   ,[Updated])
                             VALUES
                                   (@Description
                                   ,@Type
                                   ,@Date
                                   ,@Created
                                   ,@Updated)";
                await connection.ExecuteAsync(query, holiday);
            }
            return holiday;
        }

        public async Task<Holiday> UpdateAsync(Holiday holiday)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = @"UPDATE [dbo].[Holidays]
                               SET [Description] = @Description
                                  ,[Type] = @Type
                                  ,[Date] = @Date
                                  ,[Updated] = @Updated
                             WHERE [Id] = @Id";
                await connection.ExecuteAsync(query, holiday);
            }
            return holiday;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = @"DELETE FROM [dbo].[Holidays] WHERE Id = @id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<Holiday>> GetAllAsync(int pageIndex, int pageSize, string serach)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = $"SELECT TOP {pageSize+1} * FROM[dbo].[Holidays] WHERE Id NOT IN (SELECT TOP {pageSize * (pageIndex)} Id FROM[dbo].[Holidays]) AND Description LIKE '%{serach}%'";
                return await connection.QueryAsync<Holiday>(query);
            }
        }

        public async Task<Holiday> GetAsync(int id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var query = @"SELECT * FROM [dbo].[Holidays] WHERE Id = @id";
                return await connection.QueryFirstOrDefaultAsync<Holiday>(query, new { Id = id });
            }
        }

    }
}
