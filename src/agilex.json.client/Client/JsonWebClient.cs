using agilex.json.client.HeaderProviders;
using agilex.json.client.Rest;
using agilex.json.client.Urls;

namespace agilex.json.client.Client
{
    public class JsonWebClient : WebClient
    {
        public JsonWebClient(string baseUrl)
            : base(baseUrl, new JsonTypeParser())
        {
            
        }

        public JsonWebClient(string baseUrl, IHeaderAppender headerAppender)
            : base(new UrlBuilder(baseUrl), new JsonTypeParser(), headerAppender)
        {

        }
    }
}