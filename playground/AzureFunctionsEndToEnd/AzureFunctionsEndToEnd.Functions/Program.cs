// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsWebApplicationBuilder.CreateBuilder();

builder.AddServiceDefaults();

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddSource("Microsoft.Azure.Functions.Worker");
    })
    .UseFunctionsWorkerDefaults();

var host = builder.Build();
host.Run();
