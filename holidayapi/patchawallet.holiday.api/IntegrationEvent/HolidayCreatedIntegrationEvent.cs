using System;

namespace patchawallet.holiday.api
{
    public class HolidayCreatedIntegrationEvent : IntegrationEvent
    {
        public int HolidayId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public HolidayCreatedIntegrationEvent(int holidayId, string description, string type, DateTime date,
            DateTime created, DateTime updated)
        {
            HolidayId = holidayId;
            Description = description;
            Type = type;
            Date = date;
            Created = created;
            Updated = updated;
        }
    }
}
