namespace ASPNetCoreHostedServices
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Hosting;

    public class Program
    {
        public static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure((ctx, app) =>
                    {
                        if (ctx.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseHttpsRedirection();
                        app.UseRouting();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                })
                .ConfigureServices(services =>
                {
                    services.AddControllers();
                })
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
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
                             opts.ConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
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
