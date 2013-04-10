using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.IntegrationTests
{
    [TestFixture]
    public class FoodTests : TestsBase
    {
        [Test]
        public void Retrieve_FoodLogs_For_Yesterday()
        {
            List<FoodLog> foodLogs = client.GetFoodLogs(DateTime.Today.AddDays(-1), Configuration.FitbitUserId);

            Assert.IsNotNull(foodLogs);

            Console.WriteLine("Total Calories: " + foodLogs.Sum(f => f.NutritionalValues.Calories));
            foreach (var foodLog in foodLogs)
            {
                Console.WriteLine("[{0}]: {1}",
                    foodLog.LoggedFood.MealTypeId,
                    foodLog.LoggedFood.Name);
            }
        }

        [Test]
        public void Post_FoodLog_For_Today()
        {
            FoodLog foodLog = new FoodLog();
            foodLog.LoggedFood = new LoggedFood();
            foodLog.LoggedFood.FoodId = 29060;
            foodLog.LoggedFood.Calories = 82;
            foodLog.LoggedFood.MealTypeId = (int)MealType.Breakfast;
            foodLog.LoggedFood.Unit = new FoodUnit();
            foodLog.LoggedFood.Unit.Id = 204;
            foodLog.LoggedFood.Amount = 1;
            
            DateTime postedDate = DateTime.Today;

            FoodLog postedFoodLog = client.AddFoodLog(foodLog, postedDate);

            Assert.IsNotNull(postedFoodLog);
            Assert.AreEqual(postedDate, postedFoodLog.LogDate);
            Assert.AreNotEqual(0, postedFoodLog.LogId);
        }

    }
}