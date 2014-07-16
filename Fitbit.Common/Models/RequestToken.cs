using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models
{
    public class RequestToken
    {
        public string Token { get; set; }
        public string Secret { get; set; }
        public string Verifier { get; set; }

    }
}
