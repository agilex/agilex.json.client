using System.Net;

namespace agilex.json.client.Errors
{
    public class HttpWebResponseWasNull : HttpException
    {
        public HttpWebResponseWasNull(string url)
            : base(url, HttpStatusCode.InternalServerError, string.Empty)
        {
        }
    }

}