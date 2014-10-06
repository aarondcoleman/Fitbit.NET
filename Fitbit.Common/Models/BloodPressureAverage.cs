using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models
{
    public class BloodPressureAverage
    {
        public string Condition { get; set; }
        public int Diastolic { get; set; }
        public int Systolic { get; set; }
    }
}
