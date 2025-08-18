using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.Validation.Base;

public static class ValidatorsDependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        // Add your validators here
        services.AddValidatorsFromAssemblyContaining(typeof(UserValidador));
        return services;
    }
}