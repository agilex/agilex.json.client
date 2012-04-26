using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using agilex.json.client.Errors;
using agilex.json.client.HeaderProviders;

namespace agilex.json.client.Client
{
    public class WebClient : IWebClient
    {
        readonly IHeaderAppender _headerAppender;

        public WebClient(IHeaderAppender headerAppender)
        {
            _headerAppender = headerAppender;
        }

        public WebClient() : this(new NoOpHeaderAppender())
        {
        }

        #region IWebClient Members

        public void MakeWebRequest(string url, string method)
        {
            TryWebRequest(
                url, method, request =>
                                 {
                                     var response = InitiateRequest(request);
                                     ParseResponseAsString(response);
                                 });
        }

        public void MakeWebRequest<T>(string url, string method, T body)
        {
            TryWebRequest(
                url, method, request =>
                                 {
                                     AddRequestBodyAsJson(body, request);
                                     var response = InitiateRequest(request);
                                     ParseResponseAsString(response);
                                 });
        }

        public T MakeWebRequestWithResult<T>(string url, string method)
        {
            return TryWebRequest<T>(url, method, InitiateRequest);
        }

        public T MakeWebRequestWithResult<T>(string url, string method, T body)
        {
            return MakeWebRequestWithResult<T, T>(url, method, body);
        }

        public TDown MakeWebRequestWithResult<TUp, TDown>(string url, string method, TUp body)
        {
            return TryWebRequest<TDown>(
                url, method, request =>
                                 {
                                     AddRequestBodyAsJson(body, request);
                                     return InitiateRequest(request);
                                 });
        }

        #endregion

        static WebResponse InitiateRequest(WebRequest request)
        {
            var response = request.GetResponse();
            var status = ((HttpWebResponse) response).StatusCode;
            if (Convert.ToInt32(status) >= 400) throw new Exception("Error making request");
            return response;
        }

        void TryWebRequest(string url, string method, Action<WebRequest> webRequest)
        {
            try
            {
                var request = BuildRequest(url, method);
                webRequest(request);
            }
            catch (Exception ex)
            {
                throw ExtractWebResponseFromException(url, ex);
            }
        }

        TDown TryWebRequest<TDown>(string url, string method, Func<WebRequest, WebResponse> makeWebRequest)
        {
            try
            {
                var request = BuildRequest(url, method);
                var response = makeWebRequest(request);
                return ParseResponseAsString(response).FromJson<TDown>();
            }
            catch (Exception ex)
            {
                throw ExtractWebResponseFromException(url, ex);
            }
        }

        static Exception ExtractWebResponseFromException(string url, Exception exception)
        {
            if (exception.GetType() != typeof(WebException)) return exception;
            // 401, 404, 500
            var response = ((WebException) exception).Response;
            if (response == null)
            {
                return new HttpWebResponseWasNull(url);
            }

            var status = ((HttpWebResponse)response).StatusCode;
            var body = ParseResponseAsString(response);
            List<Error> errors;
            try
            {
                errors = new List<Error>{body.FromJson<Error>()};
            }
            catch 
            {
                try
                {
                    errors = body.FromJson<List<Error>>();
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

        static string ParseResponseAsString(WebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) throw new Exception("Invalid response");
                using (var reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        static void AddRequestBodyAsJson<TUp>(TUp body, WebRequest request)
        {
            request.ContentType = "application/json";
            using (var requestStream = request.GetRequestStream())
            {
                using (var writer = new JsonTextWriter(new StreamWriter(requestStream)))
                {
                    var ser = new JsonSerializer();
                    ser.Serialize(writer, body);
                }
            }
        }

        HttpWebRequest BuildRequest(string url, string method)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            _headerAppender.AppendTo(request);
            request.Method = method;
            request.KeepAlive = true;
            return request;
        }

        class NoOpHeaderAppender : IHeaderAppender
        {
            public void AppendTo(WebRequest request)
            {              
            }
        }
    }
}