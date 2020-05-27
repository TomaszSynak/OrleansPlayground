namespace Grains.HotDesk
{
    using System;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces.HotDesk;
    using Orleans;
    using Orleans.Runtime;

    public class HotDeskGrain : Grain, IHotDesk
    {
        private readonly IPersistentState<HotDeskState> _hotDeskState;

        public HotDeskGrain([PersistentState("HotDesk", "HotDesksStorage")] IPersistentState<HotDeskState> hotDeskState) => _hotDeskState = hotDeskState;

        public async Task<IImmutableList<Reservation>> GetReservations(CancellationToken cancellationToken = default)
            => await Task.FromResult(_hotDeskState.State.GetReservations());

        public async Task<bool> TryReserveHotDesk(DateTime reservationDate, string developerEmail, CancellationToken cancellationToken = default)
        {
            if (!_hotDeskState.State.TryAddReservation(reservationDate, developerEmail))
            {
                return false;
            }

            await _hotDeskState.WriteStateAsync();
            return true;
        }
    }
}
