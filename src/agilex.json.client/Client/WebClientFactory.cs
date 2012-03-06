namespace agilex.json.client.Client
{
    public class WebClientFactory : IWebClientFactory
    {
        public IWebClient Instance()
        {
            return new WebClient();
        }

        public IWebClient InstanceUsingAuthentication(string username, string password)
        {
            return new WebClient(username, password);
        }
    }
}