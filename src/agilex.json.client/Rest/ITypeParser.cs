namespace agilex.json.client.Rest
{
    public interface ITypeParser
    {
        T From<T>(string objAsString);
        string To<T>(T obj);
    }
}