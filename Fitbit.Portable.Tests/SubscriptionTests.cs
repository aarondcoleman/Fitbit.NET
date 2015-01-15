using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class SubscriptionTests
    {
        [Test]
        public void Can_Deserialize_UpdatedResource()
        {
            // aka Add Subscription response
            var content = "AddSubscriptionResponse.json".GetContent();
            var deserializer = new JsonDotNetSerializer();

            var subscription = deserializer.Deserialize<ApiSubscription>(content);
            Assert.AreEqual(APICollectionType.user, subscription.CollectionType);
            Assert.AreEqual("227YZL", subscription.OwnerId);
            Assert.AreEqual("1", subscription.SubscriberId);
            Assert.AreEqual("323", subscription.SubscriptionId);
        }

        [Test]
        public void Can_Deserialize_ApiSubscription()
        {
            var content = "ListApiSubscriptions.json".GetContent();
            var deserializer = new JsonDotNetSerializer {RootProperty = "apiSubscriptions"};

            var subscriptions = deserializer.Deserialize<List<ApiSubscription>>(content);

            Assert.IsNotNull(subscriptions);
            Assert.AreEqual(1, subscriptions.Count);
            var subscription = subscriptions.FirstOrDefault();
            Assert.AreEqual(APICollectionType.user, subscription.CollectionType);
            Assert.AreEqual("227YZL", subscription.OwnerId);
            Assert.AreEqual("1", subscription.SubscriberId);
            Assert.AreEqual("323", subscription.SubscriptionId);
        }

        [Test]
        public void Can_Deserialize_ApiNotifications()
        {
            var content = "ApiSubscriptionNotification.json".GetContent();
            var deserializer = new JsonDotNetSerializer();

            var resources = deserializer.Deserialize<List<UpdatedResource>>(content);

            Assert.IsNotNull(resources);
            Assert.AreEqual(3, resources.Count);
            Assert.AreEqual(2, resources.Count(x => x.CollectionType == APICollectionType.foods));
            Assert.AreEqual(1, resources.Count(x => x.CollectionType == APICollectionType.activities));

            var resource = resources[0];
            Assert.AreEqual(APICollectionType.foods, resource.CollectionType);
            Assert.AreEqual(new DateTime(2010, 03, 01), resource.Date);
            Assert.AreEqual("228S74", resource.OwnerId);
            Assert.AreEqual(ResourceOwnerType.User, resource.OwnerType);
            Assert.AreEqual("1234", resource.SubscriptionId);

            resource = resources[1];
            Assert.AreEqual(APICollectionType.foods, resource.CollectionType);
            Assert.AreEqual(new DateTime(2010, 03, 02), resource.Date);
            Assert.AreEqual("228S74", resource.OwnerId);
            Assert.AreEqual(ResourceOwnerType.User, resource.OwnerType);
            Assert.AreEqual("1234", resource.SubscriptionId);

            resource = resources[2];
            Assert.AreEqual(APICollectionType.activities, resource.CollectionType);
            Assert.AreEqual(new DateTime(2010, 03, 01), resource.Date);
            Assert.AreEqual("184X36", resource.OwnerId);
            Assert.AreEqual(ResourceOwnerType.User, resource.OwnerType);
            Assert.AreEqual("2345", resource.SubscriptionId);
        }
    }
}
