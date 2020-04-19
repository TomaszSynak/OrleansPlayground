namespace ASPNetCoreHostedServices.Controllers
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [ApiController]
    [Route("api/hello")]
    public class HelloWorldController : ControllerBase
    {
        private readonly IGrainFactory _client;
        private readonly IStatelessHelloWorld _grain;

        public HelloWorldController(IGrainFactory client)
        {
            _client = client;
            _grain = _client.GetGrain<IStatelessHelloWorld>(0);
        }

        [HttpGet]
        public Task<string> SayHello() => _grain.SayHello();
    }
}