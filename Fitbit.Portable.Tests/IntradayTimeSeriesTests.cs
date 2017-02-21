using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class IntradayTimeSeriesTests
    {
        [Test]
        [Category("Portable")]
        public async void GetIntraDayTimeSeriesCaloriesIntensityMetsAsync_Success()
        {
            DateTime expectedResult = new DateTime(2015, 3, 20, 0, 1, 0);

            string content = SampleDataHelper.GetContent("IntradayActivitiesCalories.json");
            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                message.Method.Should().Be(HttpMethod.Get);
                message.RequestUri.AbsoluteUri.Should().Be("https://api.fitbit.com/1/user/-/activities/calories/date/2015-03-20/1d.json");
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            var response = await fitbitClient.GetIntraDayTimeSeriesAsync(IntradayResourceType.CaloriesOut, new DateTime(2015, 3, 20), new TimeSpan(24, 0, 0));

            response.DataSet[1].Time.Should().Be(expectedResult);
            response.DataSet[1].METs.Should().Be("10");
            response.DataSet[1].Level.Should().Be("0");
            response.DataSet[1].Value.Should().Be("1.1857000589370628");
        }

        [Test]
        [Category("Portable")]
        public async void GetIntraDayTimeSeriesCaloriesIntensityMetsAsync_ReturnsNullIfMissingDateTime()
        {
            string content = SampleDataHelper.GetContent("IntradayActivitiesCaloriesMissingDateTime.json");
            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                message.Method.Should().Be(HttpMethod.Get);
                message.RequestUri.AbsoluteUri.Should().Be("https://api.fitbit.com/1/user/-/activities/calories/date/2015-03-20/1d.json");
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            IntradayData response = await fitbitClient.GetIntraDayTimeSeriesAsync(IntradayResourceType.CaloriesOut, new DateTime(2015, 3, 20), new TimeSpan(24, 0, 0));

            response.Should().Be(null);
        }

        [Test]
        [Category("Portable")]
        public async void GetIntraDayTimeSeriesStepsAsync_Success()
        {
            DateTime expectedResult = new DateTime(2016, 3, 8, 0, 1, 0);

            string content = SampleDataHelper.GetContent("IntradayActivitiesSteps.json");
            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                message.Method.Should().Be(HttpMethod.Get);
                message.RequestUri.AbsoluteUri.Should().Be("https://api.fitbit.com/1/user/-/activities/steps/date/2016-03-08/1d.json");
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            var response = await fitbitClient.GetIntraDayTimeSeriesAsync(IntradayResourceType.Steps, new DateTime(2016, 3, 8), new TimeSpan(24, 0, 0));

            response.DataSet[1].Time.Should().Be(expectedResult);
            response.DataSet[1].Value.Should().Be("2");

        }
    }
}
