// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Dcp;
using Microsoft.Extensions.DependencyInjection;

namespace Aspire.Hosting.Dashboard;

internal static class CommandsConfigurationExtensions
{
    internal static IResourceBuilder<T> WithLifeCycleCommands<T>(this IResourceBuilder<T> builder) where T : IResource
    {
        builder.WithCommand(
            "start",
            "Start",
            context => IsStopped(context.ResourceSnapshot.State?.Text) ? ResourceCommandState.Enabled : ResourceCommandState.Hidden,
            async context =>
            {
                var executor = context.ServiceProvider.GetRequiredService<ApplicationExecutor>();

                await executor.StartResourceAsync(context.ResourceName).ConfigureAwait(false);
            },
            "<path d=\"M17.22 8.69a1.5 1.5 0 0 1 0 2.62l-10 5.5A1.5 1.5 0 0 1 5 15.5v-11A1.5 1.5 0 0 1 7.22 3.2l10 5.5Zm-.48 1.75a.5.5 0 0 0 0-.88l-10-5.5A.5.5 0 0 0 6 4.5v11c0 .38.4.62.74.44l10-5.5Z\"/>",
            isHighlighted: true);

        builder.WithCommand(
            "stop",
            "Stop",
            context => !IsStopped(context.ResourceSnapshot.State?.Text) ? ResourceCommandState.Enabled : ResourceCommandState.Hidden,
            async context =>
            {
                var executor = context.ServiceProvider.GetRequiredService<ApplicationExecutor>();

                await executor.StopResourceAsync(context.ResourceName).ConfigureAwait(false);
            },
            "<path d=\"M15.5 4c.28 0 .5.22.5.5v11a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11c0-.28.22-.5.5-.5h11Zm-11-1C3.67 3 3 3.67 3 4.5v11c0 .83.67 1.5 1.5 1.5h11c.83 0 1.5-.67 1.5-1.5v-11c0-.83-.67-1.5-1.5-1.5h-11Z\"/>",
            isHighlighted: true);

        builder.WithCommand(
            "restart",
            "Restart",
            context => !IsStopped(context.ResourceSnapshot.State?.Text) ? ResourceCommandState.Enabled : ResourceCommandState.Hidden,
            async context =>
            {
                var executor = context.ServiceProvider.GetRequiredService<ApplicationExecutor>();

                await executor.StopResourceAsync(context.ResourceName).ConfigureAwait(false);
                await executor.StartResourceAsync(context.ResourceName).ConfigureAwait(false);
            },
            "<path d=\"M16 10A6 6 0 0 0 5.53 6H7.5a.5.5 0 0 1 0 1h-3a.5.5 0 0 1-.5-.5v-3a.5.5 0 0 1 1 0v1.6a7 7 0 1 1-1.98 4.36.5.5 0 0 1 1 .08L4 10a6 6 0 0 0 12 0Z\"/>",
            isHighlighted: false);

        return builder;

        static bool IsStopped(string? state) => state is "Exited" or "Finished" or "FailedToStart";
    }
}
