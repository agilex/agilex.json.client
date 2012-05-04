namespace agilex.json.client.Urls
{
    public class UrlBuilder : IUrlBuilder
    {
        readonly string _baseUrl;

        public UrlBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        #region IUrlBuilder Members

        public string Build(string urlfragment)
        {
            return string.Format("{0}/{1}", _baseUrl, urlfragment);
        }

        public string BuildPagedUrl(string urlFragment, int page, int size)
        {
            return Build(urlFragment) + string.Format("?page={0}&size={1}", page, size);
        }

        #endregion
    }
}