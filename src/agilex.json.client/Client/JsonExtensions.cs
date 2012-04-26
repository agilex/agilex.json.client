using System;
using Newtonsoft.Json;

namespace agilex.json.client.Client
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                throw new Exception("Errored reading json(" + json + ")", e);
            }
        }
    }
}