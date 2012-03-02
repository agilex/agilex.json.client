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
    }
}