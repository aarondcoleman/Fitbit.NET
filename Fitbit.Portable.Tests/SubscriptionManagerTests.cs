using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Models;
using Fitbit.Models;
using Fitbit.Portable.Tests.Helpers;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    public class SubscriptionManagerTests
    {
        [Test]
        [Category("PubSub")]
        public void Can_Process_Single_Request()
        {
            const int ExpectedResourceUpdateCount = 1;
            const string Expected_ownerId = "224F7B";
            const string Expected_subId = "21951163-8ad9-444b-8225-ff230b6e9885-activities";

            DateTime Expected_Date = new DateTime(2012, 5, 24);

            string content = SampleDataHelper.GetContent("SingleSubscriptionNotification.xml");

            var sut = new SubscriptionManager();

            List<UpdatedResource> resultResourceList = sut.ProcessUpdateReponseBody(content);
            Assert.IsNotNull(resultResourceList);
            Assert.AreEqual(ExpectedResourceUpdateCount, resultResourceList.Count);

            UpdatedResource result = resultResourceList[0];

            Assert.AreEqual(Expected_ownerId, result.OwnerId);
            Assert.AreEqual(Expected_subId, result.SubscriptionId);

            Assert.AreEqual(APICollectionType.foods, result.CollectionType);
            Assert.AreEqual(ResourceOwnerType.User, result.OwnerType);
            Assert.AreEqual(Expected_Date, result.Date);

        }

        [Test]
        [Category("PubSub")]
        public void Can_Process_Multiple_Request()
        {
            const int Expect_TWO_UpdatedResources = 2;
            const string Expected_ownerId = "2242TQ";
            const string Expected_subId = "b4b6dc1a5ead4b4e84f7b7f5b2f16b21-activities";

            DateTime Expected_Date = new DateTime(2012, 7, 25);

            string content = SampleDataHelper.GetContent("MultipleSingleSubscriptionNotification.xml");

            var sut = new SubscriptionManager();

            List<UpdatedResource> resultResourceList = sut.ProcessUpdateReponseBody(content);

            Assert.IsNotNull(resultResourceList);
            Assert.AreEqual(Expect_TWO_UpdatedResources, resultResourceList.Count);

            UpdatedResource result = resultResourceList[1];

            Assert.AreEqual(Expected_ownerId, result.OwnerId);
            Assert.AreEqual(Expected_subId, result.SubscriptionId);

            Assert.AreEqual(APICollectionType.activities, result.CollectionType);
            Assert.AreEqual(ResourceOwnerType.User, result.OwnerType);
            Assert.AreEqual(Expected_Date, result.Date);

        }
    }
}
