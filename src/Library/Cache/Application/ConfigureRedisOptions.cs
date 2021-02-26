using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace Microservice.Library.Cache.Application
{
    internal class ConfigureRedisOptions : IConfigureOptions<RedisOptions>
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly CacheGenOptions CacheGenOptions;

        public ConfigureRedisOptions(
            IServiceProvider serviceProvider,
            IOptions<CacheGenOptions> cacheGenOptionsAccessor)
        {
            ServiceProvider = serviceProvider;
            CacheGenOptions = cacheGenOptionsAccessor.Value;
        }

        public void Configure(RedisOptions options)
        {
            DeepCopy(CacheGenOptions.RedisOptions, options);
        }

        private void DeepCopy(RedisOptions source, RedisOptions target)
        {
            target = JsonConvert.DeserializeObject<RedisOptions>(JsonConvert.SerializeObject(source));
        }
    }
}
