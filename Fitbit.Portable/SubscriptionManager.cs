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

            bodyContent = StripSignatureString(bodyContent);

            var ser = new XmlSerializer(typeof(UpdatedResourceList), "");

            using (var stringReader = new StringReader(bodyContent))
            {
                var deserializedBody = (UpdatedResourceList)ser.Deserialize(stringReader);
                Debug.WriteLine(deserializedBody.Resources[0]);
                return deserializedBody.Resources;
            }
        }

        public string StripSignatureString(string bodyContent)
        {
            string sep = "<?xml";
            char[] sepChars = sep.ToCharArray();
            bodyContent = bodyContent.Substring(bodyContent.IndexOf(sep));

            string lastNodeCharacter = ">";
            int bodyEndPosition = bodyContent.LastIndexOf(lastNodeCharacter);

            bodyContent = bodyContent.Substring(0, bodyEndPosition + 1);

            return bodyContent;

        }
    }
}
