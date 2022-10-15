using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    public class AzureServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        private readonly ServiceBusConnectionStringBuilder _serviceBusConnectionStringBuilder;
        private readonly string _subscriptionClientName;
        private SubscriptionClient _subscriptionClient;
        private ITopicClient _topicClient;

        bool _disposed;

        public AzureServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder,
            string subscriptionClientName)
        {
            _serviceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ??
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            _subscriptionClientName = subscriptionClientName;
            _subscriptionClient = new SubscriptionClient(_serviceBusConnectionStringBuilder, subscriptionClientName);
            _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
        }

        public ITopicClient TopicClient
        {
            get
            {
                if (_topicClient.IsClosedOrClosing)
                {
                    _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
                }
                return _topicClient;
            }
        }

        public ISubscriptionClient SubscriptionClient
        {
            get
            {
                if (_subscriptionClient.IsClosedOrClosing)
                {
                    _subscriptionClient = new SubscriptionClient(_serviceBusConnectionStringBuilder, _subscriptionClientName);
                }
                return _subscriptionClient;
            }
        }

        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => _serviceBusConnectionStringBuilder;

        public ITopicClient CreateModel()
        {
            if (_topicClient.IsClosedOrClosing)
            {
                _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
            }

            return _topicClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;
        }

        ~AzureServiceBusPersisterConnection()
        {
            Dispose(false);
        }

    }
}
