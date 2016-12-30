using System.Collections.Generic;

namespace SeatAssignment.Entities
{
    public class ReservationAssignment
    {
        public string RequestId { get; set; }

        public List<string> AssignedSeats { get; set; }
    }
}
