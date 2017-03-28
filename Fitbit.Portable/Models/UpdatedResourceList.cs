using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Fitbit.Models;

namespace Fitbit.Api.Portable.Models
{
    [XmlRoot("updates")]
    public class UpdatedResourceList
    {
        public UpdatedResourceList() { Resources = new List<UpdatedResource>(); }

        [XmlElement("updatedResource")]
        public List<UpdatedResource> Resources { get; set; }
    }
}
