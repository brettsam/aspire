// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.ServiceDiscovery;
using Microsoft.Extensions.ServiceDiscovery.Abstractions;
using Microsoft.Extensions.ServiceDiscovery.Dns;
using Microsoft.Extensions.ServiceDiscovery.Internal;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Extension methods for configuring service discovery.
/// </summary>
public static class HostingExtensions
{
    /// <summary>
    /// Adds the core service discovery services and configures defaults.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddServiceDiscovery(this IServiceCollection services)
    {
        return services.AddServiceDiscoveryCore()
            .AddConfigurationServiceEndPointResolver()
            .AddPassThroughServiceEndPointResolver();
    }

    /// <summary>
    /// Adds the core service discovery services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddServiceDiscoveryCore(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddLogging();
        services.TryAddSingleton<TimeProvider>(static sp => TimeProvider.System);
        services.TryAddSingleton<IServiceEndPointSelectorProvider, RoundRobinServiceEndPointSelectorProvider>();
        services.TryAddSingleton<ServiceEndPointResolverFactory>();
        services.TryAddSingleton<ServiceEndPointResolverRegistry>();
        return services;
    }

    /// <summary>
    /// Configures a service discovery endpoint resolver which uses <see cref="IConfiguration"/> to resolve endpoints.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddConfigurationServiceEndPointResolver(this IServiceCollection services)
    {
        return services.AddConfigurationServiceEndPointResolver(configureOptions: null);
    }

    /// <summary>
    /// Configures a service discovery endpoint resolver which uses <see cref="IConfiguration"/> to resolve endpoints.
    /// </summary>
    /// <param name="configureOptions">The delegate used to configure the provider.</param>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddConfigurationServiceEndPointResolver(this IServiceCollection services, Action<OptionsBuilder<ConfigurationServiceEndPointResolverOptions>>? configureOptions = null)
    {
        services.AddServiceDiscoveryCore();
        services.AddSingleton<IServiceEndPointResolverProvider, ConfigurationServiceEndPointResolverProvider>();
        var options = services.AddOptions<ConfigurationServiceEndPointResolverOptions>();
        configureOptions?.Invoke(options);
        return services;
    }

    /// <summary>
    /// Configures a service discovery endpoint resolver which passes through the input without performing resolution.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddPassThroughServiceEndPointResolver(this IServiceCollection services)
    {
        services.AddServiceDiscoveryCore();
        services.AddSingleton<IServiceEndPointResolverProvider, PassThroughServiceEndPointResolverProvider>();
        return services;
    }

    /// <summary>
    /// Adds DNS-based service discovery to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">The DNS service discovery configuration options.</param>
    /// <returns>The provided <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddDnsServiceEndPointResolver(this IServiceCollection services, Action<OptionsBuilder<DnsServiceEndPointResolverOptions>>? configureOptions = null)
    {
        services.AddServiceDiscoveryCore();
        services.AddSingleton<IServiceEndPointResolverProvider, DnsServiceEndPointResolverProvider>();
        var options = services.AddOptions<DnsServiceEndPointResolverOptions>();
        configureOptions?.Invoke(options);
        return services;
    }
}
