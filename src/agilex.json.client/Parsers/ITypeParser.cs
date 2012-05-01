namespace agilex.json.client.Parsers
{
    public interface ITypeParser
    {
        T From<T>(string objAsString);
        string To<T>(T obj);
    }
}