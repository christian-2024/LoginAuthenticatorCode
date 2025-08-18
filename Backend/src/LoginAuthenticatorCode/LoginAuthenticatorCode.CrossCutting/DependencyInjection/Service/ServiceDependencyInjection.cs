using LoginAuthenticatorCode.Domain.Interfaces.Service;
using LoginAuthenticatorCode.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.Service;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}