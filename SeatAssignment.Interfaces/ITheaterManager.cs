using SeatAssignment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatAssignment.Interfaces
{
    public interface ITheaterManager
    {
        List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests);
    }
}
