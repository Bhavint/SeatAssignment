using SeatAssignment.Entities;
using System.Collections.Generic;

namespace SeatAssignment.Interfaces
{
    public interface IInputReader
    {
        List<ReservationRequest> GetTicketRequests();
    }
}
