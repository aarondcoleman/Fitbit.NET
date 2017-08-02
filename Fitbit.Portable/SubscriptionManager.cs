using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Fitbit.Api.Portable.Models;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    using System;
    using System.Collections.Generic;

    public class SubscriptionManager
    {
        public List<UpdatedResource> ProcessUpdateReponseBody(string bodyContent)
        {
            // bodyContent is json
            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<List<UpdatedResource>>(bodyContent);
        }
    }
}
