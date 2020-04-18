namespace Grains
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;

    public class HelloWorldGrain : Orleans.Grain, IHelloWorld
    {
        private readonly ILogger<HelloWorldGrain> _logger;

        public HelloWorldGrain(ILogger<HelloWorldGrain> logger)
        {
            this._logger = logger;
        }

        public async Task<string> SayHello() => await Task.FromResult("Hello world!");
    }
}