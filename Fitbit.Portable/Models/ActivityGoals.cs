using System;

namespace Fitbit.Models
{
    public class ActivityGoals
    {
        public int CaloriesOut{ get; set;}
        public int Steps { get; set; }
        public Double Distance { get; set; }

        // removed from Fitbit API:  https://groups.google.com/forum/#!topic/fitbit-api/8IRaX6RW7g4
        //public int ActiveScore { get; set; }
        public int? Floors { get; set; }
    }
}
