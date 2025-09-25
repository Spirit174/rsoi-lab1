using Microsoft.EntityFrameworkCore;

namespace Person.Server.Extensions;

public static class HostProviderExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<>()!;
        
        context.Database.Migrate();
        
        return host;
    }
}