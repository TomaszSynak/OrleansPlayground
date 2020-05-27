namespace HotDesk.Components
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [ApiController]
    [AllowAnonymous]
    [Route("api/hello")]
    [Produces("application/json")]
    public class HelloWorldController : ControllerBase
    {
        private readonly IStatelessHelloWorld _statelessHelloWorld;

        private readonly IStatefulHelloWorld _statefulHelloWorld;

        public HelloWorldController(IGrainFactory client)
        {
            _statelessHelloWorld = client.GetGrain<IStatelessHelloWorld>(0);

            _statefulHelloWorld = client.GetGrain<IStatefulHelloWorld>(0);
        }

        [HttpGet("stateless")]
        public async Task<string> StatelessHello() => await _statelessHelloWorld.SayHello();

        [HttpGet("stateful")]
        public async Task<string> StatefulHello() => await _statefulHelloWorld.SayHello();

        [HttpGet("stateful/add")]
        public async Task AddStatefulHello() => await _statefulHelloWorld.SaveGreeting();
    }
}