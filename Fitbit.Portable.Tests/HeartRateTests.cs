using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Models;
using Fitbit.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class HeartRateTests
    {
        [Test]
        [Category("Portable")]
        public async Task GetHeartRateTimeSeriesAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetHeartRateTimeSeries.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/heart/date/today/1d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetHeartRateTimeSeries("today", DateRangePeriod.OneDay);
            ValidateHeartRateTimeSeriesData(response);
        }

        [Test]
        [Category("Portable")]
        public void GetHeartRateTimeSeriesAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<HeartActivitiesTimeSeries>>> result = () => fitbitClient.GetHeartRateTimeSeries("", DateRangePeriod.OneDay);

            result.ShouldThrow<FitbitException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_HeartRateTimeSeries()
        {
            //assemble
            string content = SampleDataHelper.GetContent("GetHeartRateTimeSeries.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "activities-heart" };

            //act
            List<HeartActivitiesTimeSeries> stats = deserializer.Deserialize<List<HeartActivitiesTimeSeries>>(content);

            //assert
            ValidateHeartRateTimeSeriesData(stats);
        }

        [Test]
        [Category("Portable")]
        public async Task GetHeartRateIntradayTimeSeriesAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetHeartRateIntradayTimeSeries1.1.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1.1/user//activities/heart/date/2017-08-28/1d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetHeartRateTimeSeries("today", DateRangePeriod.OneDay);
            ValidateHeartRateTimeSeriesData(response);
        }

        [Test]
        [Category("Portable")]
        public void GetHeartRateIntradayTimeSeriesAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<HeartActivitiesIntradayTimeSeries>> result = () => fitbitClient.GetHeartRateIntraday("", HeartRateResolution.fifteenMinute);

            result.ShouldThrow<FitbitException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_HeartRateIntradayTimeSeries()
        {
            //assemble
            string content = SampleDataHelper.GetContent("GetHeartRateIntradayTimeSeries1.1.json");

            //act
            HeartActivitiesIntradayTimeSeries stats = JsonConvert.DeserializeObject<HeartActivitiesIntradayTimeSeries>(content);

            //assert
            ValidateHeartRateIntradayTimeSeriesData(stats);
        }

        private void ValidateHeartRateIntradayTimeSeriesData(HeartActivitiesIntradayTimeSeries activity)
        {
            //Activities Heart Intraday
            var actIntraday = activity.ActivitiesHeartIntraday;

            actIntraday.Dataset.Count().Should().Be(96); //Dataset count

            actIntraday.Dataset[0].Time.TimeOfDay.Should().Be(new TimeSpan(0,0,0,0)); //First entry
            actIntraday.Dataset[0].Value.Should().Be(57);

            actIntraday.Dataset[95].Time.TimeOfDay.Should().Be(new TimeSpan(0, 23,45, 0)); //Last entry
            actIntraday.Dataset[95].Value.Should().Be(47);

            actIntraday.DatasetInterval.Should().Be(15); //Dataset interval

            actIntraday.DatasetType.Should().Be("minute"); //Dataset Type

            //Activities Heart
            var act = activity.ActivitiesHeart.First();

            act.DateTime.Should().Be(new DateTime(2017, 8, 21)); //DateTime

            act.HeartRateZones.Count().Should().Be(4); //Zones Count

            act.HeartRateZones[0].CaloriesOut.Should().Be(2071.96748); //First zone
            act.HeartRateZones[0].Max.Should().Be(92);
            act.HeartRateZones[0].Min.Should().Be(30);
            act.HeartRateZones[0].Minutes.Should().Be(1387);
            act.HeartRateZones[0].Name.Should().Be("Out of Range");

            act.HeartRateZones[3].CaloriesOut.Should().Be(186.84666); //Last zone
            act.HeartRateZones[3].Max.Should().Be(220);
            act.HeartRateZones[3].Min.Should().Be(156);
            act.HeartRateZones[3].Minutes.Should().Be(14);
            act.HeartRateZones[3].Name.Should().Be("Peak");

            act.CustomHeartRateZones.Count().Should().Be(0); //Empty CustomHeart Rate Zones

            act.Value.Should().Be(55.44); //Value

        }

        private void ValidateHeartRateTimeSeriesData(List<HeartActivitiesTimeSeries> activities)
        {
            var act = activities;

            act.Count().Should().Be(7);

            var firstAct = act[0];
            firstAct.DateTime.Should().Be(new DateTime(2017, 08, 15));

            firstAct.Value.CustomHeartRateZones.Count().Should().Be(0); //Empty Custom Heart Rate Zones

            firstAct.Value.HeartRateZones.Count().Should().Be(4); //Heart Rate Zones Count

            firstAct.Value.HeartRateZones[0].CaloriesOut.Should().Be(2257.6846); //First zone
            firstAct.Value.HeartRateZones[0].Max.Should().Be(92);
            firstAct.Value.HeartRateZones[0].Min.Should().Be(30);
            firstAct.Value.HeartRateZones[0].Minutes.Should().Be(1264);
            firstAct.Value.HeartRateZones[0].Name.Should().Be("Out of Range");

            firstAct.Value.HeartRateZones[3].CaloriesOut.Should().Be(287.33136); //Last zone
            firstAct.Value.HeartRateZones[3].Max.Should().Be(220);
            firstAct.Value.HeartRateZones[3].Min.Should().Be(156);
            firstAct.Value.HeartRateZones[3].Minutes.Should().Be(21);
            firstAct.Value.HeartRateZones[3].Name.Should().Be("Peak");

            firstAct.Value.RestingHeartRate.Should().Be(51);
        }
    }
}
