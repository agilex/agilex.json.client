namespace agilex.json.client.Client
{
    public interface IWebClient
    {
        void MakeWebRequest(string url, string method);
        void MakeWebRequest<T>(string url, string method, T body);
        T MakeWebRequestWithResult<T>(string url, string method);
        T MakeWebRequestWithResult<T>(string url, string method, T body);
        TDown MakeWebRequestWithResult<TUp, TDown>(string url, string method, TUp body);
    }
}