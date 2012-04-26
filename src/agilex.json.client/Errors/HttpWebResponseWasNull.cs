using System;

namespace agilex.json.client.Errors
{
    public class HttpWebResponseWasNull : Exception
    {
        public HttpWebResponseWasNull(string url) : base(string.Format("Got null response from {0}", url))
        {
        }
    }
}