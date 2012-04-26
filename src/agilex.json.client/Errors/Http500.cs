using System.Collections.Generic;

namespace agilex.json.client.Errors
{
    public class Http500 : HttpError
    {
        public Http500(IEnumerable<Error> errors)
            : base(errors)
        {
        }
    }
}