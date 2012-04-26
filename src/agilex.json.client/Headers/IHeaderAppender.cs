using System.Net;

namespace agilex.json.client.Headers
{
    public interface IHeaderAppender
    {
        void AppendTo(WebRequest request);
    }

}