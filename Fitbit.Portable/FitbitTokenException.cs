using System.Collections.Generic;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitTokenException : FitbitException
    {
        public FitbitTokenException(IEnumerable<ApiError> errors = null, string message = default(string))
            : base(message ?? $"Token exception - see errors for more details.", errors)
        {
        }
    }
}
