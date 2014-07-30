using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Models;
using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal static class JsonDotNetSerializerExtensions
    {
        /// <summary>
        /// GetFriends has to do some custom manipulation with the returned representation
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="friendsJson"></param>
        /// <returns></returns>
        internal static List<UserProfile> GetFriends(this JsonDotNetSerializer serializer, string friendsJson)
        {
            if (string.IsNullOrWhiteSpace(friendsJson))
            {
                throw new ArgumentNullException("friendsJson", "friendsJson can not be empty, null or whitespace.");
            }

            // todo: additional error checking of json string required
            serializer.RootProperty = "user";
            var friends = JToken.Parse(friendsJson)["friends"];
            return friends.Children().Select(serializer.Deserialize<UserProfile>).ToList();           
        }
    }
}