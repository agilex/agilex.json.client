using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using agilex.json.client.Rest;

namespace agilex.json.client.Client
{
    public class WebClient : IWebClient
    {
        readonly string _password;
        readonly string _username;

        public WebClient(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public WebClient() : this(string.Empty, string.Empty)
        {
        }

        #region IWebClient Members

        public void MakeWebRequest(string url, string method)
        {
            TryWebRequest(
                url, method, request =>
                                 {
                                     if (method == RestfulClient.HttpVerbPut || method == RestfulClient.HttpVerbPost)
                                         AddEmptyBody(request);
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
            return TryWebRequest<T>(url, method, request =>
                                                     {
                                                         if (method == RestfulClient.HttpVerbPut || method == RestfulClient.HttpVerbPost)
                                                             AddEmptyBody(request);
                                                         return InitiateRequest(request);
                                                     });
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
                Trace.WriteLine(string.Format("{0}, {1}",method, url));
                var request = BuildRequest(url, method, _username, _password);
                webRequest(request);
            }
            catch (Exception ex)
            {
                throw ExtractWebResponseFromException(ex);
            }
        }

        TDown TryWebRequest<TDown>(string url, string method, Func<WebRequest, WebResponse> makeWebRequest)
        {
            try
            {
                var request = BuildRequest(url, method, _username, _password);
                var response = makeWebRequest(request);
                return ParseResponseAsJson<TDown>(response);
            }
            catch (Exception ex)
            {
                throw ExtractWebResponseFromException(ex);
            }
        }

        static Exception ExtractWebResponseFromException(Exception exception)
        {
            if (exception.GetType() == typeof (WebException))
            {
                // 401, 404, 500
                var response = ((WebException) exception).Response;
                return new Exception(
                    string.Format("Web exception, body follows:\n{0}", ParseResponseAsString(response)), exception);
            }

            return new Exception("Internal error", exception);
        }

        static TDown ParseResponseAsJson<TDown>(WebResponse response)
        {
            if (response.ContentType != "application/json")
                throw new Exception("Invalid content type, this is not json");
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) throw new Exception("Invalid response");

                using (var reader = new JsonTextReader(new StreamReader(responseStream)))
                {
                    var deserializer = new JsonSerializer();
                    return deserializer.Deserialize<TDown>(reader);
                }
            }
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

        static void AddEmptyBody(WebRequest request)
        {
           AddRequestBodyAsJson(string.Empty, request);
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

        static HttpWebRequest BuildRequest(string url, string method, string username, string password)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                AddAuthHeader(request, username, password);
            request.Method = method;
            request.KeepAlive = true;
            return request;
        }

        static void AddAuthHeader(WebRequest request, string username, string password)
        {
            if (request.Headers.AllKeys.Any(p => p.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase)))
                return;

            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
            var authHeader = string.Format("Basic {0}", token);
            request.Headers.Add("Authorization", authHeader);
        }
    }
}