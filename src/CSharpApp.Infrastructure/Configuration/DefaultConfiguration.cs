using CSharpApp.Application;
using Microsoft.Extensions.Logging;
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
        if (httpClientSettings != null)
        {
            services.Configure<HttpClientSettings>(configuration.GetSection(nameof(HttpClientSettings)));
            services.AddHttpClient<CSharpAppClient>().AddTransientHttpErrorPolicy(policy =>
            {
                return policy.WaitAndRetryAsync(httpClientSettings.RetryCount, _ => TimeSpan.FromSeconds(httpClientSettings.SleepDuration));
            });            
        }
        else
        {
            httpClientSettings = new HttpClientSettings() { LifeTime = 10, RetryCount = 2, SleepDuration = 100 };
            services.AddSingleton(httpClientSettings);
            services.AddHttpClient<CSharpAppClient>().AddTransientHttpErrorPolicy(policy =>
            {
                return policy.WaitAndRetryAsync(httpClientSettings.RetryCount, _ => TimeSpan.FromSeconds(httpClientSettings.SleepDuration));
            });            
        }

        services.AddSingleton<IProductsService, ProductsService>();
        services.AddSingleton<ICategoriesService, CategoriesService>();
        return services;
    }
}