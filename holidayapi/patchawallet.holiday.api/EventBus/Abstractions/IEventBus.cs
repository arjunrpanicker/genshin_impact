namespace patchawallet.holiday.api
{
    public interface IEventBus
    {
        bool Publish(IntegrationEvent @event);
    }
}
