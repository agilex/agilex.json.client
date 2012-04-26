using System.Net;

namespace agilex.json.client.HeaderProviders
{
    public interface IHeaderAppender
    {
        void AppendTo(WebRequest request);
    }

}