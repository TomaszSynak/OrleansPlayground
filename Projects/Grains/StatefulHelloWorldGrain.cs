namespace Grains
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Runtime;

    public class StatefulHelloWorldGrain : Grain, IStatefulHelloWorld
    {
        private readonly ILogger<StatefulHelloWorldGrain> _logger;

        private readonly IPersistentState<HelloWorldState> _helloWorldState;

        public StatefulHelloWorldGrain(ILogger<StatefulHelloWorldGrain> logger, [PersistentState("HelloWorld", "HelloWorldStore")] IPersistentState<HelloWorldState> helloWorldState)
        {
            _logger = logger;
            _helloWorldState = helloWorldState;
        }

        public async Task<string> SayHello() => await Task.FromResult(_helloWorldState.State.Greetings ?? "State is empty");

        public async Task SaveGreeting()
        {
            _helloWorldState.State.Greetings = $"Hello world from {nameof(StatefulHelloWorldGrain)}!";
            await _helloWorldState.WriteStateAsync();
        }
    }
}