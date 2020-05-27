namespace HotDesk
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Hosting;

    public class Program
    {
        private static string _connectionString = string.Empty;

        public static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure(Startup.Configure);
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddControllers();
                    _connectionString = hostBuilderContext.Configuration["StorageConnectionString"];
                })
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseAzureStorageClustering(opts =>
                        {
                            opts.ConnectionString = _connectionString;
                            opts.TableName = "OrleansSiloInstancesLocal";
                        })

                        // .ConfigureEndpoints(siloPort: 22222, gatewayPort: 30000)
                        .ConfigureLogging(opts => opts.SetMinimumLevel(LogLevel.Warning))
                        .Configure<ClusterOptions>(opts =>
                        {
                            opts.ClusterId = "dev";
                            opts.ServiceId = "HotDesksLocalApi";
                        })
                        .Configure<EndpointOptions>(opts =>
                        {
                            opts.AdvertisedIPAddress = IPAddress.Loopback;
                            opts.SiloPort = 11111;
                            opts.GatewayPort = 30000;
                        })
                        .ConfigureApplicationParts(parts =>
                        {
                            parts.AddFromApplicationBaseDirectory();
                        })
                        .AddAzureBlobGrainStorage(
                         name: "HelloWorldStore",
                         configureOptions: opts =>
                         {
                             opts.UseJson = true;
                             opts.ContainerName = "orleans-container";
                             opts.ConnectionString = _connectionString;
                         })
                        .AddAzureTableGrainStorage(
                            "HotDesksStorage",
                            opts =>
                            {
                                opts.UseFullAssemblyNames = false;
                                opts.UseJson = true;
                                opts.TableName = "HotDesks";
                                opts.ConnectionString = _connectionString;
                            })
                        .UseDashboard(opts =>
                        {
                            opts.Username = "admin";
                            opts.Password = "pass";
                            opts.Host = "*";
                            opts.Port = 8080;
                            opts.BasePath = "dashboard";
                            opts.HostSelf = true;
                            opts.CounterUpdateIntervalMs = 1000;
                        });
                })
            .RunConsoleAsync();
    }
}
