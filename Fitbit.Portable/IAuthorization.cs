using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Fitbit.Api.Portable
{
    public interface IAuthorization
    {
        void SetAuthorizationHeader(HttpClient httpClient);
    }
}
