using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatAssignment.Entities;
using SeatAssignment.Interfaces;

namespace SeatAssignment.BusinessLogic
{
    /// <summary>
    /// Ensures fairness in allotment of seats on a first come first serve basis.
    /// Tries to allocate contiguous seats as much as possible leaving 
    /// behind empty blocks of seats unless absolutely necessary to split a group.
    /// </summary>
    public class FairTheaterManager : SimpleTheaterManager, ITheaterManager
    {
        public FairTheaterManager() : base()
        { }
        public override List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var results = new List<ReservationAssignment>();

            foreach (var request in requests)
            {
                //Select appropriate row to begin seat allocation for request
                lock (_seats)
                {
                    var emptyRow = GetEmptyRow(request.NumberOfSeats);
                    var rowIndex = _seats.IndexOf(emptyRow);
                    results.Add(AssignSeats(rowIndex, request));
                }
            }
            return results.OrderBy(result => result.RequestId).ToList();
        }

        //This gives the fairness to the theater manager
        private List<bool> GetEmptyRow(int requestedNumberOfSeats)
        {
            //Get number of vacancies in each row
            var vacancies = _seats.Select(row => row.Count(isOccupied => isOccupied == false)).ToList();

            var smallestContiguousBlock = int.MaxValue;
            var largestContiguousblock = int.MinValue;
            var rowIndexOfSmallestBlock = -1;
            var rowIndexOfLargestBlock = -1;
            //Select smallest and largest contiguous vacancies that can
            //accomodate requested seats
            for (int i = 0; i < vacancies.Count; i++)
            {
                var difference = vacancies[i] - requestedNumberOfSeats;
                if (difference >= 0 && difference < smallestContiguousBlock)
                {
                    smallestContiguousBlock = difference;
                    rowIndexOfSmallestBlock = i;
                }
                if (vacancies[i] > largestContiguousblock)
                {
                    largestContiguousblock = vacancies[i];
                    rowIndexOfLargestBlock = i;
                }
            }
            var emptyRow = _seats.First(row => !row.TrueForAll(isOccupied => isOccupied == true));

            if (rowIndexOfSmallestBlock != -1)
                emptyRow = _seats[rowIndexOfSmallestBlock];
            
            else if (rowIndexOfLargestBlock != -1)
                emptyRow = _seats[rowIndexOfLargestBlock];

            return emptyRow;
        }
    }
}
