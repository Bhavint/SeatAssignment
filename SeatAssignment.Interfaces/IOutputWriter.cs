using System.Collections.Generic;

namespace SeatAssignment.Interfaces
{
    public interface IOutputWriter
    {
        bool GenerateOutput(List<Entities.ReservationAssignment> assignments);
    }
}
