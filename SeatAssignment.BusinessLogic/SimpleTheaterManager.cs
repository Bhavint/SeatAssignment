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
        protected static string[] _rowNameArray;

        private int _totalSeatsAssigned;
        private int _totalCapacity;

        //Maintain only vacancies in each row. 
        //Reduces space complexity to O(NumberOfRows)
        //Better than maintaining 2D array of O(NumberOfRows*SeatsInEachRow)
        protected int[] _vacancies;

        public SimpleTheaterManager()
        {
            _rowNameArray = new string[ConfigurationReader.NumberOfRows];
            _vacancies = new int[ConfigurationReader.NumberOfRows];
            _totalSeatsAssigned = 0;
            _totalCapacity = ConfigurationReader.NumberOfRows * ConfigurationReader.SeatsInEachRow;

            for (int i = 0; i < ConfigurationReader.NumberOfRows; i++)
            {
                var row = new List<bool>(new bool[ConfigurationReader.SeatsInEachRow]);
                _rowNameArray[i] = GetColumnNameFromIndex(i);
                _vacancies[i] = ConfigurationReader.SeatsInEachRow;
            }
        }

        private static string GetColumnNameFromIndex(int index)
        {
            var columnName = Convert.ToString((char)('A' + (index % 26)));
            while (index >= 26)
            {
                index = (index / 26) - 1;
                columnName = Convert.ToString((char)('A' + (index % 26))) + columnName;
            }
            return columnName;
        }

        public virtual List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var results = new List<ReservationAssignment>();

            foreach (var request in requests)
            {
                lock (_vacancies)
                {
                    var rowIndex = -1;
                    for (int i = 0; i < _vacancies.Count(); i++)
                    {
                        if (_vacancies[i] > 0)
                        {
                            rowIndex = i;
                        }
                    }
                    results.Add(AssignSeats(rowIndex, request));
                }
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
                    //Since we are filling left to right, column index can be calculated from the vacancy
                    var columnIndex = ConfigurationReader.SeatsInEachRow - _vacancies[rowIndex];
                    var j = 0;
                    while (columnIndex < 0)
                    {
                        //When the next row is full but tickets 
                        //for this request are not yet reserved
                        if (_vacancies[j] > 0)
                        {
                            rowIndex = j;
                            columnIndex = ConfigurationReader.SeatsInEachRow - _vacancies[rowIndex];
                        }
                        j++;
                    }
                    while (columnIndex < ConfigurationReader.SeatsInEachRow && assignedSeats < request.NumberOfSeats)
                    {
                        if (_totalSeatsAssigned >= _totalCapacity)
                            throw new OverflowException("Theater Capacity Exceeded");
                        _vacancies[rowIndex]--;
                        _totalSeatsAssigned++;
                        result.AssignedSeats.Add(string.Format("{0}{1}", _rowNameArray[rowIndex], columnIndex + 1));
                        columnIndex++;
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
