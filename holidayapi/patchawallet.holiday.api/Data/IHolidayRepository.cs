using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    public interface IHolidayRepository
    {
        Task<Holiday> AddAsync(Holiday holiday);
        Task<Holiday> UpdateAsync(Holiday holiday);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Holiday>> GetAllAsync(int pageIndex, int pageSize, string serach);
        Task<Holiday> GetAsync(int id);
    }
}
// comment1
