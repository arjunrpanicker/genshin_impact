using Autofac;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace patchawallet.holiday.api.unittest
{
    public class AzureServiceBusFixture : FixtureBase
    {
        [Fact]
        public void PublishHolidayAfterCreatedShould()
        {
            // Arrange
            var @event = new HolidayCreatedIntegrationEvent(16, "Labor day", "nacional", DateTime.Parse("2021-05-01"), DateTime.Now, DateTime.Now);
            var busMock = new Mock<IEventBus>();
            var busConnectionMock = new Mock<IServiceBusPersisterConnection>();
            var loggerMock = new Mock<ILogger<EventBusServiceBus>>();
            var subscriptionClientMock = new Mock<ISubscriptionClient>();
            var topicClientMock = new Mock<ITopicClient>();
            busConnectionMock.Setup(x => x.SubscriptionClient).Returns(subscriptionClientMock.Object);
            busConnectionMock.Setup(x => x.TopicClient).Returns(topicClientMock.Object);
            busMock.Setup(x => x.Publish(It.IsAny<HolidayCreatedIntegrationEvent>()));

            // Action
            var bus = new EventBusServiceBus(busConnectionMock.Object, loggerMock.Object);
            var result = bus.Publish(@event);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void PublishHolidayAfterUpdatedShould()
        {
            // Arrange
            var @event = new HolidayUpdatedIntegrationEvent(16, "Labor day", "nacional", DateTime.Parse("2021-05-01"), DateTime.Now, DateTime.Now);
            var busMock = new Mock<IEventBus>();
            var busConnectionMock = new Mock<IServiceBusPersisterConnection>();
            var loggerMock = new Mock<ILogger<EventBusServiceBus>>();
            var subscriptionClientMock = new Mock<ISubscriptionClient>();
            var topicClientMock = new Mock<ITopicClient>();
            busConnectionMock.Setup(x => x.SubscriptionClient).Returns(subscriptionClientMock.Object);
            busConnectionMock.Setup(x => x.TopicClient).Returns(topicClientMock.Object);
            busMock.Setup(x => x.Publish(It.IsAny<HolidayCreatedIntegrationEvent>()));

            // Action
            var bus = new EventBusServiceBus(busConnectionMock.Object, loggerMock.Object);
            var result = bus.Publish(@event);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void PublishHolidayAfterDeletedShould()
        {
            // Arrange
            var @event = new HolidayRemovedIntegrationEvent(16);
            var busMock = new Mock<IEventBus>();
            var busConnectionMock = new Mock<IServiceBusPersisterConnection>();
            var loggerMock = new Mock<ILogger<EventBusServiceBus>>();
            var subscriptionClientMock = new Mock<ISubscriptionClient>();
            var topicClientMock = new Mock<ITopicClient>();
            busConnectionMock.Setup(x => x.SubscriptionClient).Returns(subscriptionClientMock.Object);
            busConnectionMock.Setup(x => x.TopicClient).Returns(topicClientMock.Object);
            busMock.Setup(x => x.Publish(It.IsAny<HolidayCreatedIntegrationEvent>()));

            // Action
            var bus = new EventBusServiceBus(busConnectionMock.Object, loggerMock.Object);
            var result = bus.Publish(@event);

            // Assert
            Assert.True(result);
        }
    }
}
