using StackExchange.Redis;

namespace BilgeAdam.TurnOnLights.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            //var options = ConfigurationOptions.Parse("127.0.0.1:6000");
            var options = ConfigurationOptions.Parse("host.docker.internal:6000");

            options.ConnectRetry = 3;
            options.AllowAdmin = true;
            var redis = ConnectionMultiplexer.Connect(options);
            services.AddSingleton<IConnectionMultiplexer>(redis);
            return services;
        }
    }
}
