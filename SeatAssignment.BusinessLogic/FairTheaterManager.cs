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
                var emptyRow = GetEmptyRow(request.NumberOfSeats);
                var rowIndex = _seats.IndexOf(emptyRow);
                results.Add(AssignSeats(rowIndex, request));
            }
            return results;
        }

        private List<bool> GetEmptyRow(int requestedNumberOfSeats)
        {
            var vacancies = _seats.Select(x => x.Count(y => !y)).ToList();
            var smallestContiguousBlock = int.MaxValue;
            var largestContiguousblock = int.MinValue;
            var indexOfSmallestBlock = -1;
            var indexOfLargestBlock = -1;
            for (int i = 0; i < vacancies.Count; i++)
            {
                var difference = vacancies[i] - requestedNumberOfSeats;
                if (difference >= 0 && difference < smallestContiguousBlock)
                {
                    smallestContiguousBlock = difference;
                    indexOfSmallestBlock = i;
                }
                if (vacancies[i] > largestContiguousblock)
                {
                    largestContiguousblock = vacancies[i];
                    indexOfLargestBlock = i;
                }
            }

            var emptyRow = _seats.First(x => !x.TrueForAll(y => y == true));

            if (indexOfSmallestBlock != -1)
                emptyRow = _seats[indexOfSmallestBlock];
            else if (indexOfLargestBlock != -1)
                emptyRow = _seats[indexOfLargestBlock];

            return emptyRow;
        }
    }
}
