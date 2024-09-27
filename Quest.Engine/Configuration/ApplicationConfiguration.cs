using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quest.Engine.Interfaces;
using Quest.Engine.Server.Interfaces;

using Quest.Infrastructure.Context;

namespace Quest.Engine.Configuration;
public static class ApplicationConfiguration
{
    public static ServiceProvider Initialize()
    {
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.ConfigureServices()
                                                .BuildServiceProvider();

        return serviceProvider;
    }

    private static ServiceCollection ConfigureServices(this ServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddSingleton(new QuestDbContext(new DbContextOptionsBuilder<QuestDbContext>().Options));
        serviceCollection.AddSingleton<IEngine, Implementations.Engine>();
        serviceCollection.AddSingleton<IServer, Server.Implementations.Server>();
        return serviceCollection;
    }
}
