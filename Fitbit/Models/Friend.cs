using System;

namespace Fitbit.Models
{
    /// <summary>
    /// This is a needed entity for RestSharp matching of the GetFriends call. 
    /// A friend is a user profile, but in XML it is a contained node
    /// </summary>
    [Obsolete]
    public class Friend
    {
        public UserProfile User { get; set; }
    }
}