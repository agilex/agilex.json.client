using System;
using agilex.json.client.Client;
using agilex.json.client.Urls;

namespace agilex.json.client.Rest
{
    public class RestfulClient : IRestfulClient
    {
        readonly IWebClientFactory _webClientFactory;
        readonly IUrlBuilder _urlBuilder;
        readonly string _username;
        readonly string _password;
        public const string HttpVerbGet = "GET";
        public const string HttpVerbPost = "POST";
        public const string HttpVerbPut = "PUT";
        public const string HttpVerbDelete = "DELETE";

        public RestfulClient(IWebClientFactory webClientFactory, IUrlBuilder urlBuilder, string username, string password)
        {
            _webClientFactory = webClientFactory;
            _urlBuilder = urlBuilder;
            _username = username;
            _password = password;
        }

        public RestfulClient(IWebClientFactory webClientFactory, IUrlBuilder urlBuilder) : this(webClientFactory, urlBuilder, string.Empty, string.Empty)
        {
        }

        public RestfulClient(string baseUrl) : this(new WebClientFactory(), new UrlBuilder(baseUrl))
        {            
        }

        IWebClient NewWebClient
        {
            get
            {
                return (!string.IsNullOrEmpty(_username) && !(string.IsNullOrEmpty(_password)))
                           ? _webClientFactory.InstanceUsingAuthentication(_username, _password)
                           : _webClientFactory.Instance();
            }
        }

        T InvokeWebClient<T>(string urlFragment, Func<IWebClient, string, T> action)
        {
            return action(NewWebClient, _urlBuilder.Build(urlFragment));
        }
        
        void InvokeWebClient(string urlFragment, Action<IWebClient, string> action)
        {
            action(NewWebClient, _urlBuilder.Build(urlFragment));
        }

        public T Get<T>(string urlFragment)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) => webClient.MakeWebRequestWithResult<T>(fullUrl, HttpVerbGet));
        }

        public void Post<T>(string urlFragment, T body)
        {
            InvokeWebClient(urlFragment, (webclient, fullUrl) => webclient.MakeWebRequest(fullUrl, HttpVerbPost, body));
        }

        public void Put<T>(string urlFragment, T body)
        {
            InvokeWebClient(urlFragment, (webclient, fullUrl) => webclient.MakeWebRequest(fullUrl, HttpVerbPut, body));
        }

        public void Delete(string urlFragment)
        {
            InvokeWebClient(urlFragment, (webclient, fullUrl) => webclient.MakeWebRequest(fullUrl, HttpVerbDelete));
        }

        public TDown PostWithResponse<TDown>(string urlFragment)
        {
            return InvokeWebClient(urlFragment,
                                   (webClient, fullUrl) =>
                                   webClient.MakeWebRequestWithResult<TDown>(fullUrl, HttpVerbPost));
        }

        public T PostWithResponse<T>(string urlFragment, T body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) => webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPost, body));
        }

        public TDown PostWithResponse<TUp, TDown>(string urlFragment, TUp body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) => webClient.MakeWebRequestWithResult<TUp, TDown>(fullUrl, HttpVerbPost, body));
        }

        public TDown PutWithResponse<TDown>(string urlFragment)
        {
            return InvokeWebClient(urlFragment,
                       (webClient, fullUrl) =>
                       webClient.MakeWebRequestWithResult<TDown>(fullUrl, HttpVerbPut));

        }

        public T PutWithResponse<T>(string urlFragment, T body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) => webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPut, body));
        }

        public TDown PutWithResponse<TUp, TDown>(string urlFragment, TUp body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) => webClient.MakeWebRequestWithResult<TUp, TDown>(fullUrl, HttpVerbPut, body));
        }

        public T DeleteWithResponse<T>(string urlFragment)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) => webClient.MakeWebRequestWithResult<T>(fullUrl, HttpVerbDelete));
        }
    }
}