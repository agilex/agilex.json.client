using System.Collections.Generic;

namespace agilex.json.client.Errors
{
    public class Http403 : HttpError
    {
        public Http403(IEnumerable<Error> errors)
            : base(errors)
        {
        }
    }
}