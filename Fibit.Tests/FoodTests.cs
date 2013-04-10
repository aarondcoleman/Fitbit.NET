using System;
using System.Collections.Generic;
using System.IO;
using Fibit.Tests.Helpers;
using Fitbit.Models;
using NUnit.Framework;
using RestSharp;

namespace Fibit.Tests
{
    [TestFixture]
    public class FoodTests
    {
        [Test]
        public void GetFood_RestSharp_DeserializesFood()
        {
            string content = File.ReadAllText(SampleData.PathFor("GetFoodLogs.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            List<FoodLog> result = deserializer.Deserialize<List<FoodLog>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            FoodLog food = result[0];
            Assert.AreEqual(false, food.IsFavorite);
            Assert.AreEqual(new DateTime(2013, 3, 17), food.LogDate);
            Assert.AreEqual(170523831, food.LogId);

            Assert.IsNotNull(food.LoggedFood);
            Assert.AreEqual("PUBLIC", food.LoggedFood.AccessLevel);
            Assert.AreEqual(3.2, food.LoggedFood.Amount);
            Assert.AreEqual("Wegmans", food.LoggedFood.Brand);
            Assert.AreEqual(96, food.LoggedFood.Calories);
            Assert.AreEqual(31613, food.LoggedFood.FoodId);
            Assert.AreEqual("en_US", food.LoggedFood.Locale);
            Assert.AreEqual(3, food.LoggedFood.MealTypeId);
            Assert.AreEqual("Plainville No Salt Added Turkey Breast Deli", food.LoggedFood.Name);

            Assert.IsNotNull(food.LoggedFood.Unit);
            Assert.AreEqual(226, food.LoggedFood.Unit.Id);
            Assert.AreEqual("oz", food.LoggedFood.Unit.Name);
            Assert.AreEqual("oz", food.LoggedFood.Unit.Plural);
            
            Assert.IsNotNull(food.NutritionalValues);
            Assert.AreEqual(96, food.NutritionalValues.Calories);
            Assert.AreEqual(1.5, food.NutritionalValues.Carbs);
            Assert.AreEqual(1, food.NutritionalValues.Fat);
            Assert.AreEqual(0, food.NutritionalValues.Fiber);
            Assert.AreEqual(24, food.NutritionalValues.Protein);
            Assert.AreEqual(88, food.NutritionalValues.Sodium);
        }

        [Test]
        public void AddFood_RestSharp_DeserializesFood()
        {
            string content = File.ReadAllText(SampleData.PathFor("AddFoodLog.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            List<FoodLog> result = deserializer.Deserialize<List<FoodLog>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            FoodLog food = result[0];

            Assert.AreEqual(true, food.IsFavorite);
            Assert.AreEqual(new DateTime(2011, 6, 29), food.LogDate);
            Assert.AreEqual(21632, food.LogId);

            Assert.IsNotNull(food.LoggedFood);
            Assert.AreEqual("PUBLIC", food.LoggedFood.AccessLevel);
            Assert.AreEqual(2.55, food.LoggedFood.Amount);
            Assert.AreEqual(null, food.LoggedFood.Brand);
            Assert.AreEqual(370, food.LoggedFood.Calories);
            Assert.AreEqual(82294, food.LoggedFood.FoodId);
            Assert.AreEqual("en_US", food.LoggedFood.Locale);
            Assert.AreEqual(7, food.LoggedFood.MealTypeId);
            Assert.AreEqual("Chips", food.LoggedFood.Name);

            Assert.IsNotNull(food.LoggedFood.Unit);
            Assert.AreEqual(304, food.LoggedFood.Unit.Id);
            Assert.AreEqual("serving", food.LoggedFood.Unit.Name);
            Assert.AreEqual("servings", food.LoggedFood.Unit.Plural);

            Assert.IsNotNull(food.NutritionalValues);
            Assert.AreEqual(370, food.NutritionalValues.Calories);
            Assert.AreEqual(47, food.NutritionalValues.Carbs);
            Assert.AreEqual(17.5, food.NutritionalValues.Fat);
            Assert.AreEqual(5, food.NutritionalValues.Fiber);
            Assert.AreEqual(5, food.NutritionalValues.Protein);
            Assert.AreEqual(325, food.NutritionalValues.Sodium);
        }
    }
}