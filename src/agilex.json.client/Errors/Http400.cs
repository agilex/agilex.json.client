using System.Collections.Generic;

namespace agilex.json.client.Errors
{
    public class Http400 : HttpError
    {
        public Http400(IEnumerable<Error> errors) : base(errors)
        {
        }
    }
}