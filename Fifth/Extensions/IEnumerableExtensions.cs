using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fifth.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string ToJson<T>(this IEnumerable<T> collection)
        {
            return JsonConvert.SerializeObject(collection);
        }
    }
}