using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    /// <summary>
    /// This is a needed entity for RestSharp matching of the GetFriends call. 
    /// A friend is a user profile, but in XML it is a contained node
    /// </summary>
    public class Friend
    {
        public UserProfile User { get; set; }
    }
}
