using System;

namespace Fitbit.Models
{
    public class WeightGoal
    {
        public DateTime StartDate { get; set; }

        public double StartWeight { get; set; }

        public double Weight { get; set; }

        public double WeightThreshold { get; set; }

        public string GoalType { get; set; }
    }
}
