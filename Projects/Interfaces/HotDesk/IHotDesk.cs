namespace Interfaces.HotDesk
{
    using System;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHotDesk : Orleans.IGrainWithStringKey
    {
        Task<IImmutableList<Reservation>> GetReservations(CancellationToken cancellationToken = default);

        Task<bool> TryReserveHotDesk(DateTime reservationDate, string developerEmail, CancellationToken cancellationToken = default);
    }
}
