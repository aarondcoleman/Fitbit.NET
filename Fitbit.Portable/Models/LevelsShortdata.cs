using System;

namespace Fitbit.Api.Portable.Models
{
    public class LevelsShortdata
    {
        public DateTime DateTime { get; set; }
        public string Level { get; set; }
        public int Seconds { get; set; }
    }
}