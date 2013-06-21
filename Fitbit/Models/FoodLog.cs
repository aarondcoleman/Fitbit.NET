using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class FoodLog
    {
        public bool IsFavorite { get; set; }
        public DateTime LogDate { get; set; }
        public long LogId { get; set; }
        public LoggedFood LoggedFood { get; set; }
        public NutritionalValues NutritionalValues { get; set; }
    }

    public class LoggedFood
    {
        //Todo: Enumerate access level
        public string AccessLevel { get; set; }
        public float Amount { get; set; }
        public string Brand { get; set; }
        public float Calories { get; set; }
        public long FoodId { get; set; }
        public long MealTypeId { get; set; }
        //Todo: Map to a locale object
        public string Locale { get; set; }
        public string Name { get; set; }
        //Todo: Add unit and units
    }
}
