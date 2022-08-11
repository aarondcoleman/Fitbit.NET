using System;

namespace Fitbit.Models
{
    public class TemperatureSkin
    {
        public DateTime DateTime { get; set; }
        public TemperatureSkinData Value { get; set; }
        public string LogType { get; set; }
    }
}
