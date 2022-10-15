using Microsoft.Azure.ServiceBus;
using System;

namespace patchawallet.holiday.api
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ITopicClient TopicClient { get; }
        ISubscriptionClient SubscriptionClient { get; }
    }
}
