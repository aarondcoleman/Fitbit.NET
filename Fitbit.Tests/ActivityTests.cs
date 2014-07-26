using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RestSharp;
using System.IO;
using RestSharp.Deserializers;
using Moq;
using System.Net;
using Fitbit.Api;
using Fitbit.Models;
using Fibit.Tests.Helpers;

namespace Fibit.Tests
{
    [TestFixture]
    class ActivityTests
    {
        [Test]
        public void Process_XmlActivitySummaryResponse_ReturnsActivity()
        {
            //string testString = "test";
            var doc = File.ReadAllText(SampleData.PathFor("DateActivity.txt"));

            //var json = new JsonDeserializer();
            //json.RootElement = "response";

            var xml = new XmlDeserializer();
            xml.RootElement = "summary";

            var output = xml.Deserialize<Fitbit.Models.ActivitySummary>(new RestResponse { Content = doc });

            Assert.IsNotNull(output);            Assert.IsNotNull(output.CaloriesOut);
            Assert.IsNotNull(output.Distances);
            Assert.IsNotNull(output.Steps);

        }

        /// <summary>
        /// Reference Mocking IRestClient:
        /// http://www.gbogea.com/archive/2012/02
        /// </summary>
        [Test]
        public void Returns_content_if_response_is_OK()
        {
            string content = File.ReadAllText(SampleData.PathFor("DateActivity.txt"));
            
            var mock = new Mock<IRestClient>();
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            deserializer.RootElement = "summary";

            mock.Setup(x => x.Execute<ActivitySummary>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<ActivitySummary>
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = content,
                    Data = deserializer.Deserialize<ActivitySummary>(new RestResponse() { Content = content })
                });

            //var client = new GuidClient(mock.Object);
            FitbitClient fitbitClient = new FitbitClient(mock.Object); 

            var result = fitbitClient.GetDayActivitySummary(DateTime.Now);

            Assert.IsNotNull(result);
            Assert.AreEqual(12345, result.Steps);
        }

        [Test]
        public void Can_Deserialize_Activity()
        {
            string content = File.ReadAllText(SampleData.PathFor("DateActivity.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            Activity result = deserializer.Deserialize<Activity>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.AreEqual(12345, result.Summary.Steps);

            Assert.AreEqual(1, result.Activities.Count); //has 1 activity log
            Assert.AreEqual(51007, result.Activities[0].ActivityId);
            Assert.AreEqual(3783, result.Activities[0].Steps);
            Assert.AreEqual(10000, result.Goals.Steps);


        }

    }
}
