namespace SeatAssignment.Entities
{
    public class ReservationRequest
    {
        public int NumberOfSeats { get; private set; }
       
        public string RequestId { get; private set; }
        public bool IsAssigned { get; set; }

        public ReservationRequest(string id, int number)
        {
            NumberOfSeats = number;
            RequestId = id;
            IsAssigned = false;
        }
    }
}
