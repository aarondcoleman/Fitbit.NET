using System;

namespace Fitbit.Api.Portable.Models
{
    public class LevelsShortdata
    {
        public DateTime Datetime { get; set; }
        public string Level { get; set; }
        public int Seconds { get; set; }
    }
}