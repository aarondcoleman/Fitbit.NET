using System;
using System.Collections.Generic;
using System.Linq;
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
            ValidateActivity(response);
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
            var deserializer = new JsonDotNetSerializer {RootProperty = "activities"};

            List<ActivityList> stats = deserializer.Deserialize<List<ActivityList>>(content);

            ValidateActivity(stats);
        }

        private void ValidateActivity(List<ActivityList> stats)
        {
            var stat = stats.First();

            stat.ActiveDuration.Should().Be(2764000);

            stat.ActivityLevel.First().Minutes.Should().Be(0);
            stat.ActivityLevel.First().Name.Should().Be("sedentary");
            
            stat.ActivityName.Should().Be("Walk");
            stat.ActivityTypeId.Should().Be(90013);
            //stat.AverageHeartRate 
                
            stat.Calories.Should().Be(375);
            //stat.CaloriesLink
            stat.Duration.Should().Be(2764000);
            //stat.ElevationGain
            //stat.HeartRateLink
            //stat.HeartRateZones
            stat.LastModified.Should().Be(new DateTime(2017, 01, 01, 5, 3, 50));
            stat.LogId.Should().Be(5390522508);
            stat.LogType.Should().Be("auto_detected");

            stat.ManualValuesSpecified.Calories.Should().Be(false);
            stat.ManualValuesSpecified.Distance.Should().Be(false);
            stat.ManualValuesSpecified.Steps.Should().Be(false);

            stat.OriginalDuration.Should().Be(2764000);
            stat.OriginalStartTime.Should().Be(new DateTime(2017, 1, 1, 4, 14, 06));
            stat.StartTime.Should().Be(new DateTime(2017, 1, 1, 4, 14, 06));
            //stat.Steps
            stat.TcxLink.Should().Be("https://api.fitbit.com/1/user/-/activities/5390522508.tcx");
        }
    }
}

