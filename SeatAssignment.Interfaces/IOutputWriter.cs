using System.Collections.Generic;

namespace SeatAssignment.Interfaces
{
    public interface IOutputWriter
    {
        void GenerateOutput(List<Entities.ReservationAssignment> assignments);
    }
}
