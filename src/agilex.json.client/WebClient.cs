using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace agilex.json.client
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
            try
            {
                // build request
                var request = (HttpWebRequest) WebRequest.Create(url);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                    AddAuthHeader(request);
                request.ContentType = "application/json";
                request.Method = method;
                request.KeepAlive = true;

                // make request
                var response = request.GetResponse();
                var status = ((HttpWebResponse) response).StatusCode;

                if (Convert.ToInt32(status) >= 400)
                {
                    throw new Exception("Error making request");
                }
            }
            catch (WebException e)
            {
                // 401, 404, 500 etc etc
                var response = e.Response;
                throw new Exception("Error calling " + url, e);
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling " + url, ex);
            }
        }

        public void MakeWebRequest<T>(string url, T objectToUpload, string method)
        {
            try
            {
                // build request
                var request = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                    AddAuthHeader(request);
                request.ContentType = "application/json";
                request.Method = method;
                request.KeepAlive = true;
                
                // add body
                using (Stream requestStream = request.GetRequestStream())
                {
                    var writer = new JsonTextWriter(new StreamWriter(requestStream));
                    var ser = new JsonSerializer();
                    ser.Serialize(writer, objectToUpload);
                }

                // make request
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;

                if (Convert.ToInt32(status) >= 400)
                {
                    throw new Exception("Error making request");
                }
            }
            catch (WebException e)
            {
                // 401, 404, 500 etc etc
                var response = e.Response;
                throw new Exception("Error calling " + url, e);
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling " + url, ex);
            }
        }

        public T MakeWebRequestWithResult<T>(string url, string method)
        {
            try
            {
                // build request
                var request = (HttpWebRequest) WebRequest.Create(url);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                    AddAuthHeader(request);
                request.ContentType = "application/json";
                request.Method = method;
                request.KeepAlive = true;
                
                // make request
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;

                if (Convert.ToInt32(status) >= 400)
                {
                    throw new Exception("Error making request");
                }

                // parse response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null) throw new Exception("Invalid response");
                    var reader = new JsonTextReader(new StreamReader(responseStream));
                    var deserializer = new JsonSerializer();
                    return deserializer.Deserialize<T>(reader);
                }                
            }
            catch (WebException e)
            {
                // 401, 404, 500 etc etc
                var response = e.Response;
                throw new Exception("Error calling " + url, e);
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling " + url, ex);
            }
        }

        public T MakeWebRequestWithResult<T>(string url, T objectToUpload, string method)
        {
            try
            {
                // build request
                var request = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                    AddAuthHeader(request);
                request.ContentType = "application/json";
                request.Method = method;
                request.KeepAlive = true;

                // add body
                using (Stream requestStream = request.GetRequestStream())
                {
                    var writer = new JsonTextWriter(new StreamWriter(requestStream));
                    var ser = new JsonSerializer();
                    ser.Serialize(writer, objectToUpload);
                }
                
                // make request
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;

                if (Convert.ToInt32(status) >= 400)
                {
                    throw new Exception("Error making request");
                }

                // parse response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null) throw new Exception("Invalid response");
                    var reader = new JsonTextReader(new StreamReader(responseStream));
                    var deserializer = new JsonSerializer();
                    return deserializer.Deserialize<T>(reader);
                }
            }
            catch (WebException e)
            {
                // 401, 404, 500 etc etc
                var response = e.Response;
                throw new Exception("Error calling " + url, e);
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling " + url, ex);
            }
        }

        public TDown MakeWebRequestWithResult<TUp, TDown>(string url, TUp objectToUpload, string method)
        {
            try
            {
                // build request
                var request = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                    AddAuthHeader(request);
                request.ContentType = "application/json";
                request.Method = method;
                request.KeepAlive = true;

                // add body
                using (Stream requestStream = request.GetRequestStream())
                {
                    var writer = new JsonTextWriter(new StreamWriter(requestStream));
                    var ser = new JsonSerializer();
                    ser.Serialize(writer, objectToUpload);
                }

                // make request
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;

                if (Convert.ToInt32(status) >= 400)
                {
                    throw new Exception("Error making request");
                }

                // parse response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null) throw new Exception("Invalid response");
                    var reader = new JsonTextReader(new StreamReader(responseStream));
                    var deserializer = new JsonSerializer();
                    return deserializer.Deserialize<TDown>(reader);
                }
            }
            catch (WebException e)
            {
                // 401, 404, 500 etc etc
                var response = e.Response;
                throw new Exception("Error calling " + url, e);
            }
            catch (Exception ex)
            {
                throw new Exception("Error calling " + url, ex);
            }
        }
        
        public T Download<T>(string url)
        {
            return default(T);
        }

        public void UploadWithNoResponse<T>(string url, string method, T obj)
        {
            try
            {
                BuildUploadRequest(obj, url, method);
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading to " + url, ex);
            }
        }

        public TResponse Upload<TUpload, TResponse>(string url, string method, TUpload obj)
        {
            try
            {
                HttpWebRequest request = BuildUploadRequest(obj, url, method);
                return ReadResponseAsJson<TResponse>(request);
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading to " + url, ex);
            }
        }

        public TResponse Upload<TUpload, TResponse>(string url, TUpload obj)
        {
            return Upload<TUpload, TResponse>(url, "POST", obj);
        }

        public T Upload<T>(string url, string method, T obj)
        {
            return Upload<T, T>(url, method, obj);
        }

        public T Upload<T>(string url, T obj)
        {
            return Upload<T, T>(url, "POST", obj);
        }

        public void Delete(string url)
        {
        }

        #endregion

        static T ReadResponseAsJson<T>(HttpWebRequest request)
        {
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                // 401, 404, 500 etc etc
                response = e.Response;
            }
            HttpStatusCode status = ((HttpWebResponse) response).StatusCode;

            var result = new StringBuilder();
            using (Stream responseStream = response.GetResponseStream())
            {
                var buffer = new byte[1024];
                int len;
                while ((len = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    result.Append(Encoding.UTF8.GetString(buffer, 0, len));
            }
            response.Close();
            if (Convert.ToInt32(status) < 400) return result.ToString().FromJson<T>();

            // deal with errors
            throw new Exception(string.Format("Error in rest call - got http status {0}. HTML follows:\n{1}", status,
                                              result));
        }



        HttpWebRequest BuildEmptyBodyRequest(string url, string method)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                AddAuthHeader(request);
            request.Method = method;
            request.ContentLength = 0;
            request.KeepAlive = true;
            return request;
        }

        HttpWebRequest BuildUploadRequest<T>(T obj, string url, string method)
        {
            string json = obj.ToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "application/json";
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                AddAuthHeader(request);
            request.Method = method;
            request.ContentLength = data.Length;
            request.KeepAlive = true;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }
            return request;
        }

        void AddAuthHeader(HttpWebRequest request)
        {
            if (request.Headers.AllKeys.Any(p => p.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase)))
                return;
            
            string token =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _username, _password)));
            string authHeader = string.Format("Basic {0}", token);
            request.Headers.Add("Authorization", authHeader);
        }
    }
}