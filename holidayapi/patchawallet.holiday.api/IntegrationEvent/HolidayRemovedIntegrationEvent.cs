namespace patchawallet.holiday.api
{
    public class HolidayRemovedIntegrationEvent : IntegrationEvent
    {
        public int HolidayId { get; set; }

        public HolidayRemovedIntegrationEvent(int holidayId)
        {
            HolidayId = holidayId;
        }
    }
}
