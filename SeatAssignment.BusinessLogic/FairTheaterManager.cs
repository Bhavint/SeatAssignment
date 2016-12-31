using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatAssignment.Entities;
using SeatAssignment.Interfaces;

namespace SeatAssignment.BusinessLogic
{
    public class FairTheaterManager : SimpleTheaterManager, ITheaterManager
    {
        public FairTheaterManager() : base()
        { }
        public override List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var results = new List<ReservationAssignment>();

            foreach (var request in requests)
            {
                var firstEmptyRow = _seats.FirstOrDefault(x => x.FindAll(y => y == false).Count >= request.NumberOfSeats);
                if (firstEmptyRow == null)
                {
                    firstEmptyRow = _seats.First(x => !x.TrueForAll(y => y == true));
                }
                var rowIndex = _seats.IndexOf(firstEmptyRow);
                var columnIndex = _seats[rowIndex].FindIndex(x => x == false);
                results.Add(AssignSeats(rowIndex, columnIndex, request));
            }
            return results;
        }
    }
}
