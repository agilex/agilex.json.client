using System;
using System.Collections.Generic;
using System.Net;
using agilex.json.client.Errors;
using agilex.json.client.HeaderProviders;
using agilex.json.client.Rest;
using agilex.json.client.Urls;

namespace agilex.json.client.Client
{
    public class WebClient : IWebClient
    {
        readonly IRawClient _rawClient;
        readonly IUrlBuilder _urlBuilder;
        const string HttpVerbGet = "GET";
        const string HttpVerbPost = "POST";
        const string HttpVerbPut = "PUT";
        const string HttpVerbDelete = "DELETE";
        readonly ITypeParser _typeParser;
        public WebClient(IUrlBuilder urlBuilder, ITypeParser typeParser, IHeaderAppender headerAppender)
        {
            _rawClient = new RawClient(headerAppender);
            _urlBuilder = urlBuilder;
            _typeParser = typeParser;
        }

        public WebClient(string baseUrl, ITypeParser typeParser)
            : this(new UrlBuilder(baseUrl), typeParser, new NoOpHeaderAppender())
        {            
        }

        public T Get<T>(string urlFragment)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) =>
                {
                    var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbGet);
                    return _typeParser.From<T>(result);
                });
        }

        public void Post<T>(string urlFragment, T body)
        {
            InvokeWebClient(urlFragment, (webclient, fullUrl) => webclient.MakeWebRequest(fullUrl, HttpVerbPost, _typeParser.To(body)));
        }

        public void Put<T>(string urlFragment, T body)
        {
            InvokeWebClient(urlFragment, (webclient, fullUrl) => webclient.MakeWebRequest(fullUrl, HttpVerbPut, _typeParser.To(body)));
        }

        public void Delete(string urlFragment)
        {
            InvokeWebClient(urlFragment, (webclient, fullUrl) => webclient.MakeWebRequest(fullUrl, HttpVerbDelete));
        }

        public TDown PostWithResponse<TDown>(string urlFragment)
        {
            return InvokeWebClient(urlFragment,
                                   (webClient, fullUrl) => {
                                       var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPost);
                                       return _typeParser.From<TDown>(result);
                                   });
        }

        public T PostWithResponse<T>(string urlFragment, T body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) =>
                {
                    var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPost, _typeParser.To(body));
                    return _typeParser.From<T>(result);
                });
        }

        public TDown PostWithResponse<TUp, TDown>(string urlFragment, TUp body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) =>
                {
                    var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPost, _typeParser.To(body));
                    return _typeParser.From<TDown>(result);
                });
        }

        public TDown PutWithResponse<TDown>(string urlFragment)
        {
            return InvokeWebClient(urlFragment,
                       (webClient, fullUrl) =>
                       {
                           var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPut);
                           return _typeParser.From<TDown>(result);
                       });

        }

        public T PutWithResponse<T>(string urlFragment, T body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) =>
                {
                    var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPut, _typeParser.To(body));
                    return _typeParser.From<T>(result);
                });
        }

        public TDown PutWithResponse<TUp, TDown>(string urlFragment, TUp body)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) =>
                {
                    var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbPut, _typeParser.To(body));
                    return _typeParser.From<TDown>(result);
                });
        }

        public T DeleteWithResponse<T>(string urlFragment)
        {
            return InvokeWebClient(
                urlFragment, (webClient, fullUrl) =>
                {
                    var result = webClient.MakeWebRequestWithResult(fullUrl, HttpVerbDelete);
                    return _typeParser.From<T>(result);
                });
        }


        T InvokeWebClient<T>(string urlFragment, Func<IRawClient, string, T> action)
        {
            try
            {
                return action(_rawClient, _urlBuilder.Build(urlFragment));
            }
            catch (HttpException e)
            {
                throw ParseException(e);
            }
        }

        HttpError ParseException(HttpException e)
        {
            var body = e.Body;
            var status = e.StatusCode;
            List<Error> errors;
            try
            {
                errors = new List<Error> { _typeParser.From<Error>(body) };
            }
            catch
            {
                try
                {
                    errors = _typeParser.From<List<Error>>(body);
                }
                catch
                {
                    errors = new List<Error> { new Error { Key = "Message", Value = body } };
                }
            }
            if (status == HttpStatusCode.BadRequest) return new Http400(errors);
            if (status == HttpStatusCode.Unauthorized) return new Http401(errors);
            if (status == HttpStatusCode.Forbidden) return new Http403(errors);
            return new Http500(errors);

        }
        void InvokeWebClient(string urlFragment, Action<IRawClient, string> action)
        {
            try
            {
                action(_rawClient, _urlBuilder.Build(urlFragment));
            }
            catch (HttpException e)
            {
                throw ParseException(e);
            }
        }

    }
}