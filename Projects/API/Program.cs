namespace ASPNetCoreHostedServices
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
                        .UseAzureStorageClustering(options =>
                        {
                            options.ConnectionString = _connectionString;
                        })
                        .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                        .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Warning))

                        // .UseLocalhostClustering()
                        .Configure<ClusterOptions>(opts =>
                        {
                            opts.ClusterId = "dev";
                            opts.ServiceId = "HellowWorldAPIService";
                        })
                        .Configure<EndpointOptions>(opts =>
                        {
                            opts.AdvertisedIPAddress = IPAddress.Loopback;
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
