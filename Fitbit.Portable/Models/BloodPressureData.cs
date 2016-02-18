using System.Collections.Generic;

namespace Fitbit.Models
{
    public class BloodPressureData
    {
        public BloodPressureAverage Average { get; set; }
        public List<BloodPressure> BP { get; set; }
    }
}
