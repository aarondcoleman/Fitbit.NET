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
    public class WeightTests
    {
        [Test]
        [ExpectedException(typeof (Exception))]
        public async void GetWeightAsync_DateRangePeriod_ThreeMonths()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetWeightAsync(DateTime.Now, DateRangePeriod.ThreeMonths);
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public async void GetWeightAsync_DateRangePeriod_SixMonths()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetWeightAsync(DateTime.Now, DateRangePeriod.SixMonths);
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public async void GetWeightAsync_DateRangePeriod_OneYear()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetWeightAsync(DateTime.Now, DateRangePeriod.OneYear);
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public async void GetWeightAsync_DateRangePeriod_Max()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetWeightAsync(DateTime.Now, DateRangePeriod.Max);
        }

        [Test]
        public async void GetWeightAsync_OneDay_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/weight/date/2012-03-05/1d.json");

            var response = await fitbitClient.GetWeightAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneDay);

            Assert.IsTrue(response.Success);
            ValidateWeight(response.Data);
        }

        [Test]
        public async void GetWeightAsync_SevenDay_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/weight/date/2012-03-05/7d.json");

            var response = await fitbitClient.GetWeightAsync(new DateTime(2012, 3, 5), DateRangePeriod.SevenDays);

            Assert.IsTrue(response.Success);
            ValidateWeight(response.Data);
        }

        [Test]
        public async void GetWeightAsync_OneWeek_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/weight/date/2012-03-05/1w.json");

            var response = await fitbitClient.GetWeightAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneWeek);
            
            Assert.IsTrue(response.Success);
            ValidateWeight(response.Data);
        }

        [Test]
        public async void GetWeightAsync_ThirtyDays_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/weight/date/2012-03-05/30d.json");

            var response = await fitbitClient.GetWeightAsync(new DateTime(2012, 3, 5), DateRangePeriod.ThirtyDays);
            
            Assert.IsTrue(response.Success);
            ValidateWeight(response.Data);
        }

        [Test]
        public async void GetWeightAsync_OneMonth_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/weight/date/2012-03-05/1m.json");

            var response = await fitbitClient.GetWeightAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneMonth);
            
            Assert.IsTrue(response.Success);
            ValidateWeight(response.Data);
        }

        [Test]
        public async void GetWeightAsync_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/weight/date/2012-03-05.json");

            var response = await fitbitClient.GetWeightAsync(new DateTime(2012, 3, 5));
            
            Assert.IsTrue(response.Success);
            ValidateWeight(response.Data);
        }

        [Test]
        public async void GetWeightAsync_TimeSpan_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/weight/date/2012-03-05/2012-03-06.json");

            var response = await fitbitClient.GetWeightAsync(new DateTime(2012, 3, 5), new DateTime(2012, 3, 6));

            Assert.IsTrue(response.Success);
            ValidateWeight(response.Data);
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public async void GetWeightAsync_DateRange_Span_Too_Large()
        {
            var fitbitClient = Helper.CreateFitbitClient(() => new HttpResponseMessage(), (r, c) => { });
            var basedate = DateTime.Now;

            await fitbitClient.GetWeightAsync(basedate.AddDays(-35), basedate);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Throws_Exception_With_Empty_String()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetWeight(string.Empty);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Throws_Exception_With_Null_String()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetWeight(null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Throws_Exception_With_WhiteSpace()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetWeight("         ");
        }

        [Test]
        public void Can_Deserialize_Weight()
        {
            string content = SampleDataHelper.GetContent("GetWeight.json");
            var deserializer = new JsonDotNetSerializer();

            var weight = deserializer.GetWeight(content);

            ValidateWeight(weight);
        }

        private FitbitClient SetupFitbitClient(string url)
        {
            string content = SampleDataHelper.GetContent("GetWeight.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual(url, message.RequestUri.AbsoluteUri);
            });

            return Helper.CreateFitbitClient(responseMessage, verification);
        }

        private void ValidateWeight(Weight weight)
        {
            Assert.IsNotNull(weight);

            Assert.AreEqual(2, weight.Weights.Count);

            var log = weight.Weights.First();
            Assert.IsNotNull(log);

            Assert.AreEqual(new DateTime(2012, 3, 5), log.Date);
            Assert.AreEqual(1330991999000, log.LogId);
            Assert.AreEqual(73f, log.Weight);
            Assert.AreEqual(23.57f, log.Bmi);
            Assert.AreEqual(new DateTime(2012, 3, 5, 23, 59, 59).TimeOfDay, log.Time.TimeOfDay);

            weight.Weights.Remove(log);
            log = weight.Weights.First();

            Assert.IsNotNull(log);

            Assert.AreEqual(new DateTime(2012, 3, 5), log.Date);
            Assert.AreEqual(1330991999000, log.LogId);
            Assert.AreEqual(72.5f, log.Weight);
            Assert.AreEqual(22.57f, log.Bmi);
            Assert.AreEqual(new DateTime(2012, 3, 5, 21, 10, 59).TimeOfDay, log.Time.TimeOfDay);
        }
    }
}