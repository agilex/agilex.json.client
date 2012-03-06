namespace agilex.json.client.Client
{
    public interface IWebClientFactory
    {
        IWebClient Instance();
        IWebClient InstanceUsingAuthentication(string username, string password);
    }
}