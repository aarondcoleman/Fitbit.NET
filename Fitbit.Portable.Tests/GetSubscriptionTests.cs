using System.Collections.Generic;
using System.Linq;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class GetSubscriptionTests
    {
        [Test] [Category("Portable")]
        public void Can_Deserialize_ApiSubscription()
        {
            var content = SampleDataHelper.GetContent("ListApiSubscriptionsResponseSingle.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "apiSubscriptions" };

            var subscriptions = deserializer.Deserialize<List<ApiSubscription>>(content);

            Assert.IsNotNull(subscriptions);
            Assert.AreEqual(1, subscriptions.Count);
            var subscription = subscriptions.FirstOrDefault();
            Assert.AreEqual(APICollectionType.user, subscription.CollectionType);
            Assert.AreEqual("227YZL", subscription.OwnerId);
            Assert.AreEqual("1", subscription.SubscriberId);
            Assert.AreEqual("323", subscription.SubscriptionId);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_ApiSubscription_Multi()
        {
            var content = SampleDataHelper.GetContent("ListApiSubscriptionsResponseMultiple.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "apiSubscriptions" };

            var subscriptions = deserializer.Deserialize<List<ApiSubscription>>(content);

            Assert.IsNotNull(subscriptions);
            Assert.AreEqual(2, subscriptions.Count);
            var subscription = subscriptions.FirstOrDefault();
            Assert.AreEqual(APICollectionType.user, subscription.CollectionType);
            Assert.AreEqual("227YZL", subscription.OwnerId);
            Assert.AreEqual("1", subscription.SubscriberId);
            Assert.AreEqual("323", subscription.SubscriptionId);

            subscription = subscriptions.LastOrDefault();
            Assert.AreEqual(APICollectionType.user, subscription.CollectionType);
            Assert.AreEqual("227YZL", subscription.OwnerId);
            Assert.AreEqual("2", subscription.SubscriberId);
            Assert.AreEqual("3230", subscription.SubscriptionId);
        }
    }
}
