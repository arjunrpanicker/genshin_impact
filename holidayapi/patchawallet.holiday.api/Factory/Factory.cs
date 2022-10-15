using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    public static class Factory
    {
        public static List<HolidayVM> ToVM(this IEnumerable<Holiday> holidays)
        {
            var vm = new List<HolidayVM>();
            foreach (var holiday in holidays)
            {
                vm.Add(holiday.ToVM());
            }
            return vm;
        }

        public static HolidayVM ToVM(this Holiday holiday)
        {
            var vm = new HolidayVM();

            if (holiday is null)
                return null;

            vm.Id = holiday.Id;
            vm.Description = holiday.Description;
            vm.Type = holiday.Type;
            vm.Date = holiday.Date;
            vm.Updated = holiday.Updated;
            vm.Created = holiday.Created;

            return vm;
        }

        public static Holiday ToEntity(this HolidayInserOrUpdateVM vm)
        {
            var entity = new Holiday();
            if (vm is null)
                return entity;

            entity.Id = vm.Id;
            entity.Description = vm.Description;
            entity.Type = vm.Type;
            entity.Date = vm.Date;
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            return entity;
        }
    }
}
