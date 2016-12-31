using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SeatAssignment.BusinessLogic;
using SeatAssignment.Entities;
using SeatAssignment.Interfaces;
using System;

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
                var container = new UnityContainer().LoadConfiguration();

                string inputFilePath;
                if (args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
                {
                    inputFilePath = args[1];
                }
                else
                    inputFilePath = ConfigurationReader.DefaultInputFilePath;
                var inputReader = container.Resolve<IInputReader>(new ResolverOverride[] { new ParameterOverride("filePath", inputFilePath) });

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

                var theaterManager = container.Resolve<ITheaterManager>();
                var assignments = theaterManager.AssignSeats(reservationRequests);
                string outputFilePath;
                if (args.Length >= 3 && !string.IsNullOrEmpty(args[2]))
                {
                    outputFilePath = args[2];
                }
                else
                    outputFilePath = ConfigurationReader.DefaultOutputFilePath;
                var outputWriter = container.Resolve<IOutputWriter>(new ResolverOverride[] { new ParameterOverride("filePath", inputFilePath) });

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
