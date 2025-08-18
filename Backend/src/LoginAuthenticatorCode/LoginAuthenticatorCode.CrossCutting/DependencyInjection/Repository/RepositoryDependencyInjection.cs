using LoginAuthenticatorCode.Data.Repository;
using LoginAuthenticatorCode.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.Repository;

public static class RepositoryDependencyInjection
{
    public static IServiceCollection AddSqlRepositoryDependency(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>(); 
        return services;
    }
}