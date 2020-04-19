namespace Grains
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;

    public class StatefulHelloWorldGrain : Grain, IStatefulHelloWorld
    {
        private readonly ILogger<StatefulHelloWorldGrain> _logger;

        public StatefulHelloWorldGrain(ILogger<StatefulHelloWorldGrain> logger)
        {
            _logger = logger;
        }

        public async Task<string> SayHello() => await Task.FromResult($"Hello world from {nameof(StatefulHelloWorldGrain)}!");
    }
}