using System;
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
    public class IntradayTimeSeriesTests
    {

        [Test]
        public async void GetIntraDayTimeSeriesAsync_Success()
        {
            string content = SampleDataHelper.GetContent("IntradayActivitiesCalories.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/calories/date/2015-03-20/1d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetIntraDayTimeSeriesAsync(IntradayResourceType.CaloriesOut, new DateTime(2015, 3, 20), new TimeSpan(24, 0, 0));

            Assert.IsTrue(response.Success);

            Assert.AreEqual(response.Data.DataSet[1].Time, new DateTime(2015, 3, 20, 0, 1, 0));

            //ValidateDataList(response.Data);
        }
    }
}
