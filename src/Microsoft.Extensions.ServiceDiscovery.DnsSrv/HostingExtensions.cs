// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using DnsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.ServiceDiscovery.Abstractions;
using Microsoft.Extensions.ServiceDiscovery.DnsSrv;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/> to add service discovery.
/// </summary>
public static class HostingExtensions
{
    /// <summary>
    /// Adds DNS SRV service discovery to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">The DNS SRV service discovery configuration options.</param>
    /// <returns>The provided <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddDnsSrvServiceEndPointResolver(this IServiceCollection services, Action<OptionsBuilder<DnsSrvServiceEndPointResolverOptions>>? configureOptions = null)
    {
        services.AddServiceDiscoveryCore();
        services.TryAddSingleton<IDnsQuery, LookupClient>();
        services.AddSingleton<IServiceEndPointResolverProvider, DnsSrvServiceEndPointResolverProvider>();
        var options = services.AddOptions<DnsSrvServiceEndPointResolverOptions>();
        configureOptions?.Invoke(options);
        return services;
    }
}
