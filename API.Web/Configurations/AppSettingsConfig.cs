using API.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace API.WEB
{
    public static class AppSettingsConfig
    {
        public static IServiceCollection AddAppConfigurations(this IServiceCollection services, IConfiguration config)
        { 
            services.AddConfiguration<SettingConfig>(config, "SettingConfig");

            return services;
        }
    }

    public static class ConfigurationExtension
    {
        public static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string configurationTag = null) where T : class
        {
            if (string.IsNullOrEmpty(configurationTag))
            {
                configurationTag = typeof(T).Name;
            }

            var instance = Activator.CreateInstance<T>();
            new ConfigureFromConfigurationOptions<T>(configuration.GetSection(configurationTag)).Configure(instance);
            services.AddSingleton(instance);
        }
    }
}
