using agilex.json.client.Headers;
using agilex.json.client.Parsers;
using agilex.json.client.Urls;

namespace agilex.json.client.Client
{
    public class JsonWebClient : BaseWebClient
    {
        public JsonWebClient(string baseUrl)
            : base(baseUrl, new JsonTypeParser(), "application/json")
        {
            
        }

        public JsonWebClient(string baseUrl, IHeaderAppender headerAppender)
            : base(new UrlBuilder(baseUrl), new JsonTypeParser(), headerAppender, "application/json")
        {

        }
    }
}