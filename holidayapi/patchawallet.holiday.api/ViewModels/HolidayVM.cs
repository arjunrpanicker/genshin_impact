using System;

namespace patchawallet.holiday.api
{
    public class HolidayVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
