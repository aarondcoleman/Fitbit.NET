using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models
{
    public class HeartRates
    {
        public List<HeartRateSummary> Average { get; set; }
        public List<HeartRateLog> Heart { get; set; }
    }
}
