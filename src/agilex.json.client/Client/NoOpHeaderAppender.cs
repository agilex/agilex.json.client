using System.Net;
using agilex.json.client.HeaderProviders;

namespace agilex.json.client.Client
{
    public class NoOpHeaderAppender : IHeaderAppender
    {
        public void AppendTo(WebRequest request)
        {
        }
    }
}