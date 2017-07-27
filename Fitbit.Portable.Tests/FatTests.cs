using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System.Threading.Tasks;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FatTests
    {
        public Fixture fixture { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            fixture = new Fixture();
        }

        [Test]
        [Category("Portable")]
        public void GetFatAsync_DateRangePeriod_ThreeMonths()
        {
            var client = fixture.Create<FitbitClient>();

            Assert.That(new AsyncTestDelegate(async () => await client.GetFatAsync(DateTime.Now, DateRangePeriod.ThreeMonths)), Throws.ArgumentException);
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_DateRangePeriod_SixMonths()
        {
            var client = fixture.Create<FitbitClient>();
            Assert.That(
                new AsyncTestDelegate(async () => await client.GetFatAsync(DateTime.Now, DateRangePeriod.SixMonths)),
                Throws.ArgumentException
            );
        }

        [Test]
        [Category("Portable")]
        public void GetFatAsync_DateRangePeriod_OneYear()
        {
            var client = fixture.Create<FitbitClient>();
            Assert.That(
                new AsyncTestDelegate(async () => await client.GetFatAsync(DateTime.Now, DateRangePeriod.OneYear)),
                Throws.ArgumentException
                );
        }

        [Test]
        [Category("Portable")]
        public void GetFatAsync_DateRangePeriod_Max()
        {
            var client = fixture.Create<FitbitClient>();
            Assert.That(new AsyncTestDelegate(async () => await client.GetFatAsync(DateTime.Now, DateRangePeriod.Max)),
                Throws.ArgumentException
                );
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_OneDay_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/1d.json");

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneDay);

            ValidateFat(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_SevenDay_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/7d.json");

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.SevenDays);

            ValidateFat(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_OneWeek_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/1w.json");

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneWeek);

            ValidateFat(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_ThirtyDays_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/30d.json");

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.ThirtyDays);

            ValidateFat(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_OneMonth_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/1m.json");

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneMonth);

            ValidateFat(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05.json");

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5));

            ValidateFat(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetFatAsync_TimeSpan_Success()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/2012-03-06.json");

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), new DateTime(2012, 3, 6));

            ValidateFat(response);
        }

        [Test]
        [Category("Portable")]
        public void GetFatAsync_DateRange_Span_Too_Large()
        {
            var fitbitClient = Helper.CreateFitbitClient(() => new HttpResponseMessage(), (r, c) => { });
            var basedate = DateTime.Now;

            Assert.That(
                new AsyncTestDelegate(async () => await fitbitClient.GetFatAsync(basedate.AddDays(-35), basedate)),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
            );

        }

        [Test]
        [Category("Portable")]
        public void Throws_Exception_With_Empty_String()
        {
            var deserializer = new JsonDotNetSerializer();
            Assert.That(
                new TestDelegate(()=>deserializer.GetFat(string.Empty)),
                Throws.ArgumentNullException)
            ;
        }

        [Test]
        [Category("Portable")]
        public void Throws_Exception_With_Null_String()
        {
            var deserializer = new JsonDotNetSerializer();
            Assert.That(
                new TestDelegate(() => deserializer.GetFat(null)),
                Throws.ArgumentNullException
            );
        }

        [Test]
        [Category("Portable")]
        public void Throws_Exception_With_WhiteSpace()
        {
            var deserializer = new JsonDotNetSerializer();
            Assert.That(
                new TestDelegate(() => deserializer.GetFat("         ")),
                Throws.ArgumentNullException
            );
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Fat()
        {
            string content = SampleDataHelper.GetContent("GetFat.json");
            var deserializer = new JsonDotNetSerializer();

            Fat fat = deserializer.GetFat(content);

            ValidateFat(fat);
        }

        private FitbitClient SetupFitbitClient(string url)
        {
            string content = SampleDataHelper.GetContent("GetFat.json");

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

        private void ValidateFat(Fat fat)
        {
            Assert.IsNotNull(fat);

            Assert.AreEqual(2, fat.FatLogs.Count);

            var log = fat.FatLogs.First();
            Assert.IsNotNull(log);

            Assert.AreEqual(new DateTime(2012, 3, 5), log.Date);
            Assert.AreEqual(1330991999000, log.LogId);
            Assert.AreEqual(14, log.Fat);
            Assert.AreEqual(new DateTime(2012, 3, 5, 23, 59, 59).TimeOfDay, log.Time.TimeOfDay);

            fat.FatLogs.Remove(log);
            log = fat.FatLogs.First();

            Assert.IsNotNull(log);

            Assert.AreEqual(new DateTime(2012, 3, 5), log.Date);
            Assert.AreEqual(1330991999000, log.LogId);
            Assert.AreEqual(13.5, log.Fat);
            Assert.AreEqual(new DateTime(2012, 3, 5, 21, 20, 59).TimeOfDay, log.Time.TimeOfDay);

        }
    }
}