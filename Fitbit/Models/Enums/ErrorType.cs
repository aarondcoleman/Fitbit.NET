using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public enum ErrorType
    {
        [StringValue("validation")]
        Validation,
        [StringValue("oauth")]
        OAuth,
        [StringValue("request")]
        Request,
        [StringValue("not_found")]
        NotFound,
        [StringValue("system")]
        System
    }
}
