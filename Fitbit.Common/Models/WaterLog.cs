using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models
{
    public class WaterLog
    {
        public long LogId { get; set; }
        public float Amount { get; set; } //amount of LIQUIDS units in the selected unit system
    }
}
