using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fitbit.Models;
using RestSharp;

namespace Fitbit.Api
{
    public class SubscriptionManager
    {
        public List<UpdatedResource> ProcessUpdateReponseBody(string bodyContent)
        {

            bodyContent = StripSignatureString(bodyContent);

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            List<UpdatedResource> result = deserializer.Deserialize<List<UpdatedResource>>(new RestResponse() { Content = bodyContent });

            return result;

        }

        public string StripSignatureString(string bodyContent)
        {
            string sep = "<?xml";
            char[] sepChars = sep.ToCharArray();
            bodyContent = bodyContent.Substring(bodyContent.IndexOf(sep));

            string lastNodeCharacter = ">";
            int bodyEndPosition = bodyContent.LastIndexOf(lastNodeCharacter );

            bodyContent = bodyContent.Substring(0, bodyEndPosition + 1);

            return bodyContent;

        }
    }
}
