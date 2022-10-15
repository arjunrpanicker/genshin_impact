using System;
using System.ComponentModel.DataAnnotations;

namespace patchawallet.holiday.api
{
    public class HolidayInserOrUpdateVM
    {
        public int Id { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
