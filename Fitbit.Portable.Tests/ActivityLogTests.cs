using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class ActivityLogTests
    {
        [Test]
        [Category("Portable")]
        public async void GetActivityLogsListAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetActivityLogsList.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/list.json?afterDate=2017-01-01T00:00:00&sort=asc&limit=20&offset=0", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetActivityLogsList(null, new DateTime(2017, 1, 1));
            //ValidateBloodPressureData(response);
        }

        [Test]
        [Category("Portable")]
        public void GetActivityLogsListAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<ActivityList>>> result = () => fitbitClient.GetActivityLogsList(null, new DateTime(2017, 1, 1));

            result.ShouldThrow<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_ActivityLogsList()
        {
            string content = SampleDataHelper.GetContent("GetActivityLogsList.json");
            var deserializer = new JsonDotNetSerializer();

            List<ActivityList> stats = deserializer.Deserialize<List<ActivityList>>(content);

            //ValidateActivity(stats);
        }
    }
}

