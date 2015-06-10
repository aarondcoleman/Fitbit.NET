using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Api.Portable
{
    public class OAuth2AccessToken
    {
            public string Token { get; set; }
            public string TokenType { get; set; } // "Bearer" is expected
            public int ExpiresIn { get; set; } //maybe convert this to a DateTime ?
            public string RefreshToken { get; set; }
    }
}
