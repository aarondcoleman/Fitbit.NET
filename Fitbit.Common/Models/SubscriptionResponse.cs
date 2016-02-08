using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class SubscriptionResponse
    {
        public string CollectionType { get; set; }
        public string OwnerId { get; set; }
        public string OwnerType { get; set; }
        public string SubscriberId { get; set; }
        public string SubscriptionId { get; set; }
    }
}
