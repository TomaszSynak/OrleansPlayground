namespace Grains.HotDesk
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Interfaces;
    using Interfaces.HotDesk;

    [Serializable]
    public class HotDeskState
    {
        public IDictionary<string, ReservationState> Reservations { get; } = new Dictionary<string, ReservationState>();

        public IImmutableList<Reservation> GetReservations()
        {
            return Reservations
                .Select(r => new Reservation(r.Value.ReservationDate, r.Value.DeveloperEmail))
                .ToImmutableList();
        }

        public bool TryAddReservation(DateTime reservationDate, string developerEmail)
        {
            var reservationKey = GetReservationKey(reservationDate);

            if (Reservations.ContainsKey(reservationKey))
            {
                return false;
            }

            Reservations.Add(reservationKey, new ReservationState(reservationDate, developerEmail));
            return true;
        }

        private static string GetReservationKey(DateTime reservationDate) => $"{reservationDate:dd-MM-yyyy}";
    }
}
