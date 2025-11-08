using CSharpApp.Application;
using Polly;

namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services.Configure<RestApiSettings>(configuration!.GetSection(nameof(RestApiSettings)));

        var httpClientSettings = configuration.GetSection(nameof(HttpClientSettings)).Get<HttpClientSettings>();
        services.AddSingleton(httpClientSettings);
        services.AddHttpClient<CSharpAppClient>().AddTransientHttpErrorPolicy(policy =>
        {
            return policy.WaitAndRetryAsync(httpClientSettings.RetryCount, _ => TimeSpan.FromSeconds(httpClientSettings.SleepDuration));
        });
        services.Configure<HttpClientSettings>(configuration.GetSection(nameof(HttpClientSettings)));

        services.AddSingleton<IProductsService, ProductsService>();

        return services;
    }
}