using SeatAssignment.BusinessLogic;
using SeatAssignment.Entities;
using System;
using System.Collections.Generic;

namespace SeatAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /*
                 * 1. Read Input
                 * 2. Assign Seats
                 * 3. Return Results
                */
                string inputFilePath;
                if (!string.IsNullOrEmpty(args[1]))
                {
                    inputFilePath = args[1];
                }
                else
                    inputFilePath = ConfigurationReader.DefaultInputFilePath;
                var inputReader = new FileInputReader(inputFilePath);

                var reservationRequests = inputReader.GetTicketRequests();

                var validationResult = InputValidator.Validate(reservationRequests);
                if (validationResult.Status == ValidationStatus.Failure)
                {
                    Console.WriteLine("The Input does not match expectations");
                    foreach (var message in validationResult.Messages)
                    {
                        Console.WriteLine(message);
                    }
                    Console.WriteLine("Aborting...");
                    return;
                }


                var assignments = new List<ReservationAssignment>();
                string outputFilePath;
                if (!string.IsNullOrEmpty(args[2]))
                {
                    outputFilePath = args[2];
                }
                else
                    outputFilePath = ConfigurationReader.DefaultOutputFilePath;
                var outputWriter = new FileOutputWriter(outputFilePath);

                outputWriter.GenerateOutput(assignments);

            }
            catch (Exception ex)
            {
                //TODO: Handle Exceptions
                Console.WriteLine("Aborting...");
                return;
            }
        }
    }
}
