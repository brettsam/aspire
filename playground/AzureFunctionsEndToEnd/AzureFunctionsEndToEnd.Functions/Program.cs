using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .AddServiceDefaults()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddOpenTelemetry()
            .UseFunctionsWorkerDefaults()
            .UseOtlpExporter();
    })
    .Build();

host.Run();
