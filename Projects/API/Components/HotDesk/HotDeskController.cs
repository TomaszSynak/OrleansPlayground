namespace HotDesk.Components.HotDesk
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using Interfaces.HotDesk;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class HotDeskController : ControllerBase
    {
        private readonly IGrainFactory _client;

        public HotDeskController(IGrainFactory client)
        {
            _client = client;
        }

        [HttpGet("{hotDeskId}")]
        public async Task<IActionResult> GetHotDeskReservation(string hotDeskId, CancellationToken cancellationToken)
        {
            var hotDesk = _client.GetGrain<IHotDesk>(hotDeskId);
            return StatusCode((int)HttpStatusCode.OK, await hotDesk.GetReservations(cancellationToken));
        }

        [HttpPost("{hotDeskId}/reserve")]
        public async Task<IActionResult> ReserveHotDesk(string hotDeskId, HotDeskReservationDto reservationDto, CancellationToken cancellationToken)
        {
            var hotDesk = _client.GetGrain<IHotDesk>(hotDeskId);

            var reserved = await hotDesk.TryReserveHotDesk(reservationDto.ReservationDate, reservationDto.DeveloperEmail, cancellationToken);

            return reserved
                ? StatusCode((int)HttpStatusCode.Created)
                : StatusCode((int)HttpStatusCode.Conflict);
        }
    }
}
