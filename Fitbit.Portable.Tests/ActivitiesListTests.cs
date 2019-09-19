using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using Fitbit.Api.Portable.Models;
// ReSharper disable PossibleNullReferenceException

namespace Fitbit.Portable.Tests
{
    public class ActivitiesListTests
	{
        [Test]
        [Category("Portable")]
        public async Task ActivitiesListTests_Success()
        {
            string content = SampleDataHelper.GetContent("ActivitiesList.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                message.Method.Should().Be(HttpMethod.Get);
                message.RequestUri.AbsoluteUri.Should().StartWith("https://api.fitbit.com/1/user/-/activities/list.json");
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetActivitiesListAsync(DateTime.Now.AddYears(-1), DateTypeEnum.BeforeDate, SortEnum.Asc);

            ValidateActivitiesList(response);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Activities()
        {
            string content = SampleDataHelper.GetContent("ActivitiesList.json");
            var deserializer = new JsonDotNetSerializer();

            ActivitiesList stats = deserializer.Deserialize<ActivitiesList>(content);

            ValidateActivitiesList(stats);
        }
        
        private void ValidateActivitiesList(ActivitiesList results)
        {
	        results.Activities.Length.Should().Be(20);
	        results.Activities.FirstOrDefault().ActiveDuration.Should().Be(1791000);
	        results.Activities.FirstOrDefault().ActivityName.Should().Be("Sport");
	        results.Activities.FirstOrDefault().HeartRateZones.Length.Should().Be(4);
	        results.Activities.FirstOrDefault().ManualValuesSpecified.Calories.Should().Be(false);
	        results.Activities.FirstOrDefault().TcxLink.Should().NotBeNull();
        }
    }
}
