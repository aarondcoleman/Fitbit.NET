using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Fitbit.Api.Portable
{
    [Obsolete("Marking as obsolete in preparation for delition in future checkin as new constructor paradigm will not require this class.")]
    public interface IAuthorization
    {
        HttpClient ConfigureHttpClientAUthorization(HttpClient client);
    }
}
