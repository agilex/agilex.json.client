using System;
using System.Collections.Generic;

namespace agilex.json.client.Errors
{
    public class HttpError : Exception
    {
        readonly IEnumerable<Error> _errors;

        public HttpError(IEnumerable<Error> errors)
        {
            _errors = errors;
        }

        public IEnumerable<Error> Errors
        {
            get { return _errors; }
        }
    }
}
