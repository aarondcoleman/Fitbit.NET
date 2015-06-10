using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Api.Portable
{
    public interface ITokenStore 
    { 
        string Read();  
 
        void Write(string bearerToken, string refreshToken, DateTime expiration); // Maybe a TimeSpan instead? 
    } 
}