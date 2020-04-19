namespace Grains
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;

    public class StatelessHelloWorldGrain : Grain, IStatelessHelloWorld
    {
        private readonly ILogger<StatelessHelloWorldGrain> _logger;

        public StatelessHelloWorldGrain(ILogger<StatelessHelloWorldGrain> logger)
        {
            _logger = logger;
        }

        public async Task<string> SayHello() => await Task.FromResult($"Hello world from {nameof(StatelessHelloWorldGrain)}!");
    }
}