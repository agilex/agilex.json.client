namespace agilex.json.client.Rest
{
    public interface IRestfulClient
    {
        T Get<T>(string urlFragment);
        void Post<T>(string urlFragment, T body);
        void Put<T>(string urlFragment, T body);
        void Delete(string urlFragment);
        T PostWithResponse<T>(string urlFragment, T body);
        TDown PostWithResponse<TUp, TDown>(string urlFragment, TUp body);
        T PutWithResponse<T>(string urlFragment, T body);        
        TDown PutWithResponse<TUp, TDown>(string urlFragment, TUp body);
        T DeleteWithResponse<T>(string urlFragment);
    }
}