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
                                       client.Download<T>(urlBuilder.Build(urlFragment)));
        }

        public void Delete(string urlFragment)
        {
            try
            {
                NewWebClient.Delete(_urlBuilder.Build(urlFragment));
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
                NewWebClient.UploadWithNoResponse(_urlBuilder.Build(urlFragment), "PUT", up);
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
                client.Upload<TUp, TDown>(
                    urlBuilder.Build(urlFragment), "POST", up));
        }

        public TDown Put<TUp, TDown>(string urlFragment, TUp up)
        {
            return InvokeRestfulAction(
                (client, urlBuilder) =>
                client.Upload<TUp, TDown>(
                    urlBuilder.Build(urlFragment), "PUT", up));
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