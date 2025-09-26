using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Person.DataBase.Context;
using FluentValidation.AspNetCore;
using Person.DTO.Models;

namespace Person.Server.Extensions;

public static class HostProviderExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<PersonContext>()!;
        
        context.Database.Migrate();
        
        return host;
    }
    
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<PersonContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
    }
    
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}