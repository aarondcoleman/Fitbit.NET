using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Api.Portable
{
    public interface IOAuth2AccessTokenRepository
    {
        //presumably some concrete implementation will implement a constructor with an id schema

        void Add(OAuth2AccessToken accessToken);

        void Update(OAuth2AccessToken accessToken);
        
        void Delete();
        OAuth2AccessToken Get();
    }
}
