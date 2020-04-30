namespace ASPNetCoreHostedServices.Components.Health
{
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;

    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;

        public HealthController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        private static string ApiVersion
            => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty;

        /// <summary>
        /// Endpoint to verify server's responsiveness.
        /// </summary>
        /// <param name="cancellationToken"> Cancellation Token to pass </param>
        /// <response code="200"> Server is responsive </response>
        /// <response code="424"> Server is unresponsive </response>
        [HttpGet]
        [ProducesResponseType(typeof(HealthStateDto), (int)HttpStatusCode.OK)]
        public IActionResult Health(CancellationToken cancellationToken)
        {
            var healthState = new HealthStateDto
            {
                Name = _hostEnvironment.ApplicationName,
                Environment = _hostEnvironment.EnvironmentName,
                ApiVersion = ApiVersion,
                IsHealthy = true
            };

            return StatusCode((int)HttpStatusCode.OK, healthState);
        }
    }
}
