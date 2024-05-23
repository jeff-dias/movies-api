using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Movies.Application.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static Task WarmAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken)
        {
            return cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value, GetJsonSerializerOptions()), cancellationToken);
        }

        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
        {
            var cachedResult = cache.Get(key);
            value = default;

            if (cachedResult != null)
            {
                value = JsonSerializer.Deserialize<T>(cachedResult, GetJsonSerializerOptions());
                return true;
            }

            return false;
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
