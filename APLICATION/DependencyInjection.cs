using APPLICATION.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            //config.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
        });

        return services;
    }
}
