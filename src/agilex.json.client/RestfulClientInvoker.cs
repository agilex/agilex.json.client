using System;

namespace agilex.json.client
{
    public class RestfulClientInvoker
    {
        readonly IRestfulWebClientFactory _restfulWebClientFactory;
        readonly IUrlBuilder _urlBuilder;

        public RestfulClientInvoker(string baseUrl, string username, string password)
        {
            _restfulWebClientFactory = new RestfulWebClientFactory(username, password);
            _urlBuilder = new UrlBuilder(baseUrl);
        }

        public RestfulClientInvoker(string baseUrl) : this(baseUrl, string.Empty, string.Empty)
        {
            _urlBuilder = new UrlBuilder(baseUrl);
        }

        IWebClient NewWebClient
        {
            get { return _restfulWebClientFactory.Instance(); }
        }

        public T Get<T>(string urlFragment)
        {
            return InvokeRestfulAction((client, urlBuilder) =>
                                       client.MakeWebRequestWithResult<T>(urlBuilder.Build(urlFragment), "GET"));
        }

        public void Delete(string urlFragment)
        {
            try
            {
                NewWebClient.MakeWebRequest(_urlBuilder.Build(urlFragment), "DELETE");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not perform restful action", ex);
            }
        }

        public void Put<T>(string urlFragment, T up)
        {
            try
            {
                NewWebClient.MakeWebRequest(_urlBuilder.Build(urlFragment), up, "PUT");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not perform restful action", ex);
            }
        }

        public T Post<T>(string urlFragment, T up)
        {
            return Post<T, T>(urlFragment, up);
        }

        public TDown Post<TUp, TDown>(string urlFragment, TUp up)
        {
            return InvokeRestfulAction(
                (client, urlBuilder) =>
                client.MakeWebRequestWithResult<TUp, TDown>(urlBuilder.Build(urlFragment), up, "POST"));
        }

        public TDown Put<TUp, TDown>(string urlFragment, TUp up)
        {
            return InvokeRestfulAction(
                (client, urlBuilder) =>
                client.MakeWebRequestWithResult<TUp, TDown>(urlBuilder.Build(urlFragment), up, "PUT"));
        }

        T InvokeRestfulAction<T>(Func<IWebClient, IUrlBuilder, T> restfulAction)
        {
            try
            {
                return restfulAction(NewWebClient, _urlBuilder);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not perform restful action", ex);
            }
        }
    }
}