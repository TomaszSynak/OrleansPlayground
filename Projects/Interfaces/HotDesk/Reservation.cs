namespace Interfaces.HotDesk
{
    using System;

    public class Reservation
    {
        public Reservation(DateTime reservationDate, string developerEmail)
        {
            ReservationDate = reservationDate;
            DeveloperEmail = developerEmail;
        }

        public DateTime ReservationDate { get; }

        public string DeveloperEmail { get; }
    }
}
