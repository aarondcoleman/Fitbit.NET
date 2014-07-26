using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class AuthCredential
    {
        public string AuthToken { get; set; }
        public string AuthTokenSecret { get; set; }
        public string UserId { get; set; }
    }
}
