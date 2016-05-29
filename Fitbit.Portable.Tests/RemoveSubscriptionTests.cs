using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using NUnit.Framework;
using Fitbit.Models;

namespace Fitbit.Portable.Tests
{        
    [TestFixture] 
    public class RemoveSubscriptionTests
    {
        [Test]
        [Category("PubSub")]
        [Category("Portable")]
        public void DeleteSubscription_Correctly()
        {
            var subId = "320";
            var expectedUrl = @"https://api.fitbit.com/1/user/-/apiSubscriptions/"+subId+".json";

            var sut = this.SetupFitbitClient(null, expectedUrl, HttpMethod.Delete);

            //Any unexpected behavior will throw exception or fail on request checks (in handler)
            //Pass APICollectionType.user to delete subscriptions for all data sets
            sut.DeleteSubscriptionAsync(APICollectionType.user, subId).Wait();
        }

        [Test]
        [Category("PubSub")]
        [Category("Portable")]
        public void DeleteSubscriptonFromSpecificCollection()
        {
            var subId = "320";
            var collection = APICollectionType.activities;
            var expectedUrl = @"https://api.fitbit.com/1/user/-/"+ collection +@"/apiSubscriptions/" + subId + ".json";

            var sut = this.SetupFitbitClient(null, expectedUrl, HttpMethod.Delete);

            //Any unexpected behavior will throw exception or fail on request checks (in handler)
            sut.DeleteSubscriptionAsync(collection, subId).Wait();

        }

        private FitbitClient SetupFitbitClient(string contentPath, string url, HttpMethod expectedMethod, Action<HttpRequestMessage> additionalChecks = null)
        {
            var content = string.Empty;

            if (contentPath != null)
                content = SampleDataHelper.GetContent(contentPath);

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent(content) };
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
