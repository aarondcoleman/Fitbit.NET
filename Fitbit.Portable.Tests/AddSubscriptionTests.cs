using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    [Category("PubSub")]
    public class AddSubscriptionTests
    {
        private const string SubScriberId = "my_subscriber_id";

        //POST /1/user/-/apiSubscriptions/320.json
        //POST /1/user/-/activities/apiSubscriptions/320-activities.json
        //POST /1/user/-/foods/apiSubscriptions/320-foods.json
        //POST /1/user/-/sleep/apiSubscriptions/320-sleep.json
        //POST /1/user/-/body/apiSubscriptions/320-body.json

        //https://wiki.fitbit.com/display/API/Fitbit+Subscriptions+API#FitbitSubscriptionsAPI-Addasubscription
        [Test]
        [Category("PubSub")]
        [Category("Portable")]
        public async void AddSubscription_UserEndPoint_WithoutSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(0, message.Headers.Count());
            };
            
            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/apiSubscriptions/323.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.user, "323");

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_UserEndPoint_WithSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(1, message.Headers.Count());
                Assert.IsTrue(message.Headers.Contains(Constants.Headers.XFitbitSubscriberId));

                IEnumerable<string> headerValues;
                Assert.IsTrue(message.Headers.TryGetValues(Constants.Headers.XFitbitSubscriberId, out headerValues));

                Assert.AreEqual(SubScriberId, headerValues.First());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/apiSubscriptions/323.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.user, "323", SubScriberId);

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_ActivitiesEndPoint_WithoutSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(0, message.Headers.Count());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/activities/apiSubscriptions/323-activities.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.activities, "323");

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_ActivitiesEndPoint_WithSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(1, message.Headers.Count());
                Assert.IsTrue(message.Headers.Contains(Constants.Headers.XFitbitSubscriberId));

                IEnumerable<string> headerValues;
                Assert.IsTrue(message.Headers.TryGetValues(Constants.Headers.XFitbitSubscriberId, out headerValues));

                Assert.AreEqual(SubScriberId, headerValues.First());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/activities/apiSubscriptions/323-activities.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.activities, "323", SubScriberId);

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_BodyEndPoint_WithoutSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(0, message.Headers.Count());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/body/apiSubscriptions/323-body.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.body, "323");

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_BodyEndPoint_WithSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(1, message.Headers.Count());
                Assert.IsTrue(message.Headers.Contains(Constants.Headers.XFitbitSubscriberId));

                IEnumerable<string> headerValues;
                Assert.IsTrue(message.Headers.TryGetValues(Constants.Headers.XFitbitSubscriberId, out headerValues));

                Assert.AreEqual(SubScriberId, headerValues.First());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/body/apiSubscriptions/323-body.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.body, "323", SubScriberId);
            
            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_FoodEndPoint_WithoutSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(0, message.Headers.Count());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/foods/apiSubscriptions/323-foods.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.foods, "323");

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_FoodEndPoint_WithSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(1, message.Headers.Count());
                Assert.IsTrue(message.Headers.Contains(Constants.Headers.XFitbitSubscriberId));

                IEnumerable<string> headerValues;
                Assert.IsTrue(message.Headers.TryGetValues(Constants.Headers.XFitbitSubscriberId, out headerValues));

                Assert.AreEqual(SubScriberId, headerValues.First());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/foods/apiSubscriptions/323-foods.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.foods, "323", SubScriberId);
            
            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_MealsEndPoint_WithoutSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(0, message.Headers.Count());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/meals/apiSubscriptions/323-meals.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.meals, "323");

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_MealsEndPoint_WithSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(1, message.Headers.Count());
                Assert.IsTrue(message.Headers.Contains(Constants.Headers.XFitbitSubscriberId));

                IEnumerable<string> headerValues;
                Assert.IsTrue(message.Headers.TryGetValues(Constants.Headers.XFitbitSubscriberId, out headerValues));

                Assert.AreEqual(SubScriberId, headerValues.First());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/meals/apiSubscriptions/323-meals.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.meals, "323", SubScriberId);

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_SleepEndPoint_WithoutSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(0, message.Headers.Count());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/sleep/apiSubscriptions/323-sleep.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.sleep, "323");

            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_SleepEndPoint_WithSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(1, message.Headers.Count());
                Assert.IsTrue(message.Headers.Contains(Constants.Headers.XFitbitSubscriberId));

                IEnumerable<string> headerValues;
                Assert.IsTrue(message.Headers.TryGetValues(Constants.Headers.XFitbitSubscriberId, out headerValues));

                Assert.AreEqual(SubScriberId, headerValues.First());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/sleep/apiSubscriptions/323-sleep.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.sleep, "323", SubScriberId);
            
            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_WeightEndPoint_WithoutSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(0, message.Headers.Count());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/weight/apiSubscriptions/323-weight.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.weight, "323");
            
            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public async void AddSubscription_WeightEndPoint_WithSubscriberId()
        {
            Action<HttpRequestMessage> additionalChecks = message =>
            {
                Assert.AreEqual(1, message.Headers.Count());
                Assert.IsTrue(message.Headers.Contains(Constants.Headers.XFitbitSubscriberId));

                IEnumerable<string> headerValues;
                Assert.IsTrue(message.Headers.TryGetValues(Constants.Headers.XFitbitSubscriberId, out headerValues));

                Assert.AreEqual(SubScriberId, headerValues.First());
            };

            var fitbitClient = SetupFitbitClient("AddSubscriptionResponse.json", "https://api.fitbit.com/1/user/-/weight/apiSubscriptions/323-weight.json", HttpMethod.Post, additionalChecks);

            var response = await fitbitClient.AddSubscriptionAsync(APICollectionType.weight, "323", SubScriberId);
            
            Assert.AreEqual("323", response.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public void Can_Deserialize_UpdatedResource()
        {
            // aka Add Subscription response
            var content = SampleDataHelper.GetContent("AddSubscriptionResponse.json");
            var deserializer = new JsonDotNetSerializer();

            var subscription = deserializer.Deserialize<ApiSubscription>(content);
            Assert.AreEqual(APICollectionType.user, subscription.CollectionType);
            Assert.AreEqual("227YZL", subscription.OwnerId);
            Assert.AreEqual("1", subscription.SubscriberId);
            Assert.AreEqual("323", subscription.SubscriptionId);
        }

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public void Can_Deserialize_ApiSubscription()
        {
            var content = SampleDataHelper.GetContent("ListApiSubscriptions.json");
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

        [Category("PubSub")]
        [Test] [Category("Portable")]
        public void Can_Deserialize_ApiNotifications()
        {
            var content = SampleDataHelper.GetContent("ApiSubscriptionNotification.json");
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

        private FitbitClient SetupFitbitClient(string contentPath, string url, HttpMethod expectedMethod, Action<HttpRequestMessage> additionalChecks = null)
        {
            string content = SampleDataHelper.GetContent(contentPath);

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(expectedMethod, message.Method);
                Assert.AreEqual(url, message.RequestUri.AbsoluteUri);
                if (additionalChecks != null)
                    additionalChecks(message);
            });

            return Helper.CreateFitbitClient(responseMessage, verification);
        }
    }
}
