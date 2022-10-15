using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    public interface IHolidayService
    {
        Task<List<HolidayVM>> GetAllAsync(int pageIndex = 1, int pageSize = 10, string search = "");
        Task<HolidayVM> GetByIdAsync(int id);
        Task<HolidayVM> PostAsync(HolidayInserOrUpdateVM requestVM);
        Task<HolidayVM> UpdateAsync(HolidayInserOrUpdateVM requestVM);
        Task<bool> DeleteAsync(int id);
    }
}
