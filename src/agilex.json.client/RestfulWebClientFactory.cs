namespace agilex.json.client
{
    public class RestfulWebClientFactory : IRestfulWebClientFactory
    {
        readonly string _username;
        readonly string _password;

        public RestfulWebClientFactory(string username, string password)
        {
            _username = username;
            _password = password;
        }

        #region IRestfulWebClientFactory Members

        public IWebClient Instance(string username, string password)
        {
            return new WebClient(username, password);
        }

        public IWebClient Instance()
        {
            return (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
                       ? new WebClient()
                       : new WebClient(_username, _password);
        }

        #endregion
    }
}