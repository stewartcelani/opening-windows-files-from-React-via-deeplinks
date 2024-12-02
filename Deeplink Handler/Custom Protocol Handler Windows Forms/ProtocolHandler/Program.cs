using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog((context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext();
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<ProtocolHandlerService>();
        services.AddSingleton<App>();
    })
    .Build();

await host.Services.GetRequiredService<App>().RunAsync(args);