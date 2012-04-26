using System.Net;

namespace agilex.json.client.Headers
{
    public class NoOpHeaderAppender : IHeaderAppender
    {
        public void AppendTo(WebRequest request)
        {
        }
    }
}