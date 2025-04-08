using APPLICATION.Abstractions.Caching;
using APPLICATION.Abstractions.Clock;
using APPLICATION.Abstractions.Data;
using DOMAIN.Movements;
using DOMAIN.Wallets;
using INFRAESTRUCTURE.Cache;
using INFRAESTRUCTURE.Clock;
using INFRAESTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace INFRAESTRUCTURE;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        AddPersistence(services, configuration);

        AddCaching(services, configuration);

        return services;

    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Cache") ??
                    throw new ArgumentNullException(nameof(configuration));
        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }
    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("wallet_testdb");
        //Guard clause for posibly null configuration.
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException(nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>(
            options => options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention());

        // Repositories

        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IMovementRepository, MovementRepository>();


        // UnitOfWork
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());


    }
}

