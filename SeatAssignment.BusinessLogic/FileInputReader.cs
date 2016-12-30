using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SeatAssignment.Entities;
using System.IO;

namespace SeatAssignment.BusinessLogic
{
    public class FileInputReader : IInputReader
    {
        public string FilePath { get; private set; }
        public List<ReservationRequest> GetTicketRequests()
        {
            try
            {
                var reservationRequests = new List<ReservationRequest>();
                var lines = ReadLines();
                foreach (var line in lines)
                {
                    var lineElements = line.Split(' ');
                    var request = new ReservationRequest(lineElements[0], Convert.ToInt32(lineElements[1]));
                    reservationRequests.Add(request);
                }
                return reservationRequests;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured while reading from file", ex);
            }
        }

        public FileInputReader(string filePath)
        {
            FilePath = filePath;
        }

        private List<string> ReadLines()
        {
            var lines = new List<string>();
            var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }
    }
}
