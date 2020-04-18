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
        private readonly IHelloWorld _grain;

        public HelloWorldController(IGrainFactory client)
        {
            _client = client;
            _grain = _client.GetGrain<IHelloWorld>(0);
        }

        [HttpGet]
        public Task<string> SayHello() => this._grain.SayHello();
    }
}