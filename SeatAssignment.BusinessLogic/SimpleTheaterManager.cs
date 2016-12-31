using SeatAssignment.Entities;
using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatAssignment.BusinessLogic
{
    public class SimpleTheaterManager : ITheaterManager
    {
        protected static string[] _alphabetArray = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        protected List<List<bool>> _seats;

        public SimpleTheaterManager()
        {
            _seats = new List<List<bool>>();
            for (int i = 0; i < ConfigurationReader.NumberOfRows; i++)
            {
                var row = new List<bool>(new bool[ConfigurationReader.SeatsInEachRow]);
                _seats.Add(row);
            }
        }

        public virtual List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var results = new List<ReservationAssignment>();

            foreach (var request in requests)
            {
                var firstEmptyRow = _seats.First(x => !x.TrueForAll(y => y == true));
                var rowIndex = _seats.IndexOf(firstEmptyRow);
                results.Add(AssignSeats(rowIndex, request));
            }
            return results;
        }

        protected ReservationAssignment AssignSeats(int rowIndex, ReservationRequest request)
        {
            try
            {
                var result = new ReservationAssignment() { RequestId = request.RequestId, AssignedSeats = new List<string>() };
                var assignedSeats = 0;
                while (request.NumberOfSeats - assignedSeats != 0)
                {
                    var i = _seats[rowIndex].FindIndex(x => x == false);
                    if (i < 0)
                    {
                        //Corner cases when the theatre is almost full
                        var emptyRow = _seats.First(x => !x.TrueForAll(y => y == true));
                        rowIndex = _seats.IndexOf(emptyRow);
                        i = _seats[rowIndex].FindIndex(x => x == false);
                    }
                    while (i < ConfigurationReader.SeatsInEachRow && assignedSeats < request.NumberOfSeats)
                    {
                        _seats[rowIndex][i] = true;
                        result.AssignedSeats.Add(string.Format("{0}{1}", _alphabetArray[rowIndex], i + 1));
                        i++;
                        assignedSeats++;
                    }
                    rowIndex = (rowIndex + 1) % ConfigurationReader.NumberOfRows;
                }
                request.IsAssigned = true;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured while assigning seats", ex);
            }
        }
    }
}
