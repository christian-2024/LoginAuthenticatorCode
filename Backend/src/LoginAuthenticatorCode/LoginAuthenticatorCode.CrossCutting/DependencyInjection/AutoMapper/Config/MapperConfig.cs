using Microsoft.Extensions.DependencyInjection;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.AutoMapper.Config;

public static class MapperConfig
{
    public static IServiceCollection AddMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        return services;
    }
}   