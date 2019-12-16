using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Fitbit.Api.Portable.Models;
using Fitbit.Models;
using System.Collections.Generic;

namespace Fitbit.Api.Portable
{  
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
