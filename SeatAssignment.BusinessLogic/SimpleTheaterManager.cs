using SeatAssignment.Entities;
using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatAssignment.BusinessLogic
{
    /// <summary>
    /// Ensures perfect utilization of the available seats.
    /// Assigns seats strictly serially even if a group of people is split acrross multiple rows
    /// </summary>
    public class SimpleTheaterManager : ITheaterManager
    {
        protected static string[] _rowNameArray = new string[ConfigurationReader.NumberOfRows];

        protected List<List<bool>> _seats;

        public SimpleTheaterManager()
        {
            _seats = new List<List<bool>>();
            for (int i = 0; i < ConfigurationReader.NumberOfRows; i++)
            {
                var row = new List<bool>(new bool[ConfigurationReader.SeatsInEachRow]);
                _seats.Add(row);
                _rowNameArray[i] = GetColumnNameFromIndex(i);
            }
        }

        private static string GetColumnNameFromIndex(int column)
        {
            var col = Convert.ToString((char)('A' + (column % 26)));
            while (column >= 26)
            {
                column = (column / 26) - 1;
                col = Convert.ToString((char)('A' + (column % 26))) + col;
            }
            return col;
        }

        public virtual List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var results = new List<ReservationAssignment>();

            foreach (var request in requests)
            {
                var firstEmptyRow = _seats.First(row => !row.TrueForAll(isOccupied => isOccupied == true));
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
                    var i = _seats[rowIndex].FindIndex(isOccupied => isOccupied == false);
                    if (i < 0)
                    {
                        //When the next row is full but tickets 
                        //for this request are not yet reserved
                        var emptyRow = _seats.First(row => !row.TrueForAll(isOccupied => isOccupied == true));
                        rowIndex = _seats.IndexOf(emptyRow);
                        i = _seats[rowIndex].FindIndex(isOccupied => isOccupied == false);
                    }
                    while (i < ConfigurationReader.SeatsInEachRow && assignedSeats < request.NumberOfSeats)
                    {
                        _seats[rowIndex][i] = true;
                        result.AssignedSeats.Add(string.Format("{0}{1}", _rowNameArray[rowIndex], i + 1));
                        i++;
                        assignedSeats++;
                    }
                    //Circularly move to next row
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
