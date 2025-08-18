using LoginAuthenticatorCode.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.DbConfig;

public static class DbConfigDependency
{
    public static IServiceCollection AddSqlServerDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LoginAuthenticatorContext>(options =>
        {
            var vConnectionString = configuration["ConnectionString"];                  //Aqui que é definida a string de conexão com o banco de dados de qual se deseja utilizar.
            //options.UseSqlServer(vConnectionString).LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging(); //Aqui esta o SqlServer sendo utilizado como banco de dados.
            options.UseMySql(vConnectionString, ServerVersion.AutoDetect(vConnectionString))
                   .LogTo(Console.WriteLine, LogLevel.Information)
                   .EnableSensitiveDataLogging(); // Aqui está o MySQL sendo utilizado como banco de dados.
        });

        return services;
    }

    public static void UpdateDatabase(this IServiceCollection services, IApplicationBuilder app)
    {
        var seconds = 60;
        var minutes = 20;
        var commandTimeout = seconds * minutes;

        using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<LoginAuthenticatorContext>())
            {
                context?.Database.SetCommandTimeout(commandTimeout);

                if (context is not null && context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
                context?.Database.SetCommandTimeout(null);
            }
        }
    }
}