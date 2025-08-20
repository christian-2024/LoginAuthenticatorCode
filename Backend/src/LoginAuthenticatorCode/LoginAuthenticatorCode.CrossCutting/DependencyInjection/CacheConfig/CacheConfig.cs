using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.CacheConfig;

public static class CacheConfig
{
    public static IServiceCollection AddCacheServerDependency(this IServiceCollection services, IConfiguration configuration)
    {
        var vConnectionCacheString = configuration["ConnectionCacheString"];

        services.AddSingleton<IConnectionMultiplexer>(options =>
            ConnectionMultiplexer.Connect(vConnectionCacheString)
        );

        return services;
    }
}