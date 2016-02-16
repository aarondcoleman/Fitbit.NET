using System.Collections.Generic;

namespace Fitbit.Api.Portable.Models
{
    public class Food
    {
        public FoodSummary Summary { get; set; }
        public List<FoodLog> Foods { get; set; }
        public FoodGoals Goals { get; set; }
    }
}