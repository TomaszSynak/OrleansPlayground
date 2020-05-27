namespace Grains.HotDesk
{
    using System;

    public class ReservationState
    {
        public ReservationState(DateTime reservationDate, string developerEmail)
        {
            ReservationDate = reservationDate;
            DeveloperEmail = developerEmail;
        }

        public DateTime ReservationDate { get; }

        public string DeveloperEmail { get; }
    }
}
