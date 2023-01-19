using System.Data;

namespace patchawallet.holiday.api
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
//.... 
//Comment line 1
