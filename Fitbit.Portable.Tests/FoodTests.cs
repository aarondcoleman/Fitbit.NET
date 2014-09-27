using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FoodTests
    {
        [Test]
        public async void GetFoodAsync_Success()
        {
            string content = "GetFoodLogs.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/foods/log/date/2014-09-27.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFoodAsync(new DateTime(2014, 9, 27));
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();
            
            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFoodData(response.Data);
        }

        [Test]
        public async void GetFoodAsync_Errors()
        {
            string content = "GetFoodLogs.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/ghjhg/foods/log/date/2014-09-27.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFoodAsync(new DateTime(2014, 9, 27));
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
        }

        [Test]
        public void Can_Deserialize_Food()
        {
            string content = "GetFoodLogs.json".GetContent();
            var deserializer = new JsonDotNetSerializer();

            Food food = deserializer.Deserialize<Food>(content);

            ValidateFoodData(food);
        }

        private void ValidateFoodData(Food food)
        {
            Assert.IsNotNull(food);    

            Assert.IsNotNull(food.Foods);
            Assert.IsNotNull(food.Summary);
            Assert.IsNotNull(food.Goals);

            // goals
            Assert.AreEqual(2286, food.Goals.Calories);

            // summary
            Assert.AreEqual(752, food.Summary.Calories);
            Assert.AreEqual(66.5, food.Summary.Carbs);
            Assert.AreEqual(49, food.Summary.Fat);
            Assert.AreEqual(0.5, food.Summary.Fiber);
            Assert.AreEqual(12.5, food.Summary.Protein);
            Assert.AreEqual(186, food.Summary.Sodium);
            Assert.AreEqual(0, food.Summary.Water);

            // foods
            Assert.AreEqual(1, food.Foods.Count);
            var f = food.Foods.First();

            Assert.IsTrue(f.IsFavorite);
            Assert.AreEqual(new DateTime(2011, 6, 29), f.LogDate);
            Assert.AreEqual(1820, f.LogId);
            Assert.IsNotNull(f.LoggedFood);
            Assert.IsNotNull(f.NutritionalValues);

            // todo: further parsing of child objects

        }
    }
}