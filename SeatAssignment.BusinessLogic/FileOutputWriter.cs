using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SeatAssignment.Entities;
using System.IO;

namespace SeatAssignment.BusinessLogic
{
    public class FileOutputWriter : IOutputWriter
    {
        public string FilePath { get; private set; }

        public FileOutputWriter(string filePath)
        {
            FilePath = filePath;
        }

        public bool GenerateOutput(List<ReservationAssignment> assignments)
        {
            try
            {
                var fileStringBuilder = new StringBuilder();

                foreach (var assignment in assignments)
                {
                    fileStringBuilder.AppendLine(string.Format("{0} {1}", assignment.RequestId, string.Join(",", assignment.AssignedSeats.ToArray())));
                }
                using (var writer = new StreamWriter(FilePath,false))
                {
                    writer.WriteLine(fileStringBuilder.ToString());
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occured while writing to file", ex);
            }
            return true;
        }
    }
}
