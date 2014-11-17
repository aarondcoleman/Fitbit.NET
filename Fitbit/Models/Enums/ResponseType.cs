using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models.Enums
{
    public enum ResponseType
    {
        [StringValue("xml")]
        Xml = 1,
        [StringValue("json")]
        Json = 2
    }
}
