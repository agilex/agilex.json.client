using System.Collections.Generic;

namespace agilex.json.client.Errors
{
    public class Http401 : HttpError
    {
        public Http401(IEnumerable<Error> errors)
            : base(errors)
        {
        }
    }
}