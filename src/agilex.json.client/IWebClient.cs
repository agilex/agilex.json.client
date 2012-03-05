namespace agilex.json.client
{
    public interface IWebClient
    {
        T Download<T>(string url);
        void UploadWithNoResponse<T>(string url, string method, T obj);
        TResponse Upload<TUpload, TResponse>(string url, string method, TUpload obj);
        TResponse Upload<TUpload, TResponse>(string url, TUpload obj);
        T Upload<T>(string url, string method, T obj);
        T Upload<T>(string url, T obj);
        void Delete(string url);
        void MakeWebRequest(string url, string method);
        void MakeWebRequest<T>(string url, T objectToUpload, string method);
        T MakeWebRequestWithResult<T>(string url, string method);
        T MakeWebRequestWithResult<T>(string url, T objectToUpload, string method);
        TDown MakeWebRequestWithResult<TUp, TDown>(string url, TUp objectToUpload, string method);
    }
}