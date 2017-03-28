using System;
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
    public class SleepTests
    {
        [Test] [Category("Portable")]
        public async void GetSleepAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetSleep.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/sleep/date/2014-10-17.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetSleepAsync(new DateTime(2014, 10, 17));
            
            ValidatSleep(response);
        }

        [Test] [Category("Portable")]
        public void GetUserProfileAsync_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            Func<Task<SleepData>> result = () => fitbitClient.GetSleepAsync(new DateTime(2014, 11, 11));

            result.ShouldThrow<FitbitException>();
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_Sleep()
        {
            string content = SampleDataHelper.GetContent("GetSleep.json");
            var deserializer = new JsonDotNetSerializer();

            SleepData sleep = deserializer.Deserialize<SleepData>(content);

            ValidatSleep(sleep);
        }

        private void ValidatSleep(SleepData sleep)
        {
            // general
            Assert.IsNotNull(sleep);
            Assert.IsNotNull(sleep.Summary);
            Assert.IsNotNull(sleep.Sleep);

            // summary
            var summary = sleep.Summary;
            Assert.AreEqual(518, summary.TotalMinutesAsleep);
            Assert.AreEqual(2, summary.TotalSleepRecords);
            Assert.AreEqual(540, summary.TotalTimeInBed);

            // logs
            Assert.AreEqual(2, sleep.Sleep.Count);
            var l = sleep.Sleep.First();

            Assert.AreEqual(true, l.IsMainSleep);
            Assert.AreEqual(29744, l.LogId);
            Assert.AreEqual(98, l.Efficiency);
            Assert.AreEqual(new DateTime(2011, 6, 16), l.StartTime);
            Assert.AreEqual(28800000, l.Duration);
            Assert.AreEqual(0, l.MinutesToFallAsleep);
            Assert.AreEqual(480, l.MinutesAsleep);
            Assert.AreEqual(0, l.MinutesAwake);
            Assert.AreEqual(0, l.MinutesAfterWakeup);
            // awakenings count - depcrecated
            Assert.AreEqual(0, l.AwakeCount);
            Assert.AreEqual(0, l.AwakeDuration);
            Assert.AreEqual(0, l.RestlessCount);
            Assert.AreEqual(0, l.RestlessDuration);
            Assert.AreEqual(480, l.TimeInBed);
            Assert.AreEqual(3, l.MinuteData.Count);

            var min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900,1,1).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(3, min.Value);
            l.MinuteData.Remove(min);

            min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900, 1, 1, 0, 1, 0).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(2, min.Value);
            l.MinuteData.Remove(min);

            min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900, 1, 1, 0, 2, 0).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(1, min.Value);

            sleep.Sleep.Remove(l);
            l = sleep.Sleep.First();

            Assert.AreEqual(false, l.IsMainSleep);
            Assert.AreEqual(29745, l.LogId);
            Assert.AreEqual(93, l.Efficiency);
            Assert.AreEqual(new DateTime(2011, 6, 16, 14,0,0), l.StartTime);
            Assert.AreEqual(3600000, l.Duration);
            Assert.AreEqual(20, l.MinutesToFallAsleep);
            Assert.AreEqual(38, l.MinutesAsleep);
            Assert.AreEqual(0, l.MinutesAwake);
            Assert.AreEqual(2, l.MinutesAfterWakeup);
            // awakenings count - depcrecated
            Assert.AreEqual(0, l.AwakeCount);
            Assert.AreEqual(0, l.AwakeDuration);
            Assert.AreEqual(0, l.RestlessCount);
            Assert.AreEqual(0, l.RestlessDuration);
            Assert.AreEqual(60, l.TimeInBed);
            Assert.AreEqual(1, l.MinuteData.Count);

            min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900, 1, 1, 14, 0, 0).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(3, min.Value);
        }
    }
}