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
                lock (_vacancies)
                {
                    var rowIndex = GetEmptyRowIndex(request.NumberOfSeats);
                    results.Add(AssignSeats(rowIndex, request));
                }
            }
            return results.OrderBy(result => result.RequestId).ToList();
        }

        //This gives the fairness to the theater manager
        private int GetEmptyRowIndex(int requestedNumberOfSeats)
        {
            var smallestContiguousBlock = int.MaxValue;
            var largestContiguousblock = int.MinValue;
            var rowIndexOfSmallestBlock = -1;
            var rowIndexOfLargestBlock = -1;
            var rowIndexOfFirstEmptyRow = -1;
            //Select smallest and largest contiguous vacancies that can
            //accomodate requested seats

            for (int i = 0; i < _vacancies.Count(); i++)
            {
                var difference = _vacancies[i] - requestedNumberOfSeats;
                if (_vacancies[i] > 0 && rowIndexOfFirstEmptyRow != -1)
                {
                    rowIndexOfFirstEmptyRow = i;
                }
                if (difference >= 0 && difference < smallestContiguousBlock)
                {
                    smallestContiguousBlock = difference;
                    rowIndexOfSmallestBlock = i;
                }
                if (_vacancies[i] > largestContiguousblock)
                {
                    largestContiguousblock = _vacancies[i];
                    rowIndexOfLargestBlock = i;
                }
            }

            var emptyRow = rowIndexOfFirstEmptyRow;

            if (rowIndexOfSmallestBlock != -1)
                emptyRow = rowIndexOfSmallestBlock;

            else if (rowIndexOfLargestBlock != -1)
                emptyRow = rowIndexOfLargestBlock;

            return emptyRow;
        }
    }
}
