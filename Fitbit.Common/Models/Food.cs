using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class Food
    {
        public FoodSummary Summary { get; set; }
        public List<FoodLog> Foods { get; set; }
        public FoodGoals Goals { get; set; }
    }
}
