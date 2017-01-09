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
                //Inversion of Control Container
                var container = new UnityContainer().LoadConfiguration();

                //Determine input file path from arguments or pick default from configuration
                string inputFilePath;
                if (args.Length >= 1 && !string.IsNullOrEmpty(args[0]))
                {
                    inputFilePath = args[0];
                }
                else
                    inputFilePath = ConfigurationReader.DefaultInputFilePath;
                var inputReader = container.Resolve<IInputReader>(new ResolverOverride[] { new ParameterOverride("filePath", inputFilePath) });

                //Read requests from file
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

                //Create Appropriate theater Manager from the several options available.
                //Which one to pick is mentioned in configuration file.
                var theaterManager = container.Resolve<ITheaterManager>("mainTheaterManager");

                //assign seats
                var assignments = theaterManager.AssignSeats(reservationRequests);

                //create output file
                string outputFilePath;
                if (args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
                {
                    outputFilePath = args[1];
                }
                else
                    outputFilePath = ConfigurationReader.DefaultOutputFilePath;
                var outputWriter = container.Resolve<IOutputWriter>(new ResolverOverride[] { new ParameterOverride("filePath", outputFilePath) });

                if (outputWriter.GenerateOutput(assignments))
                    Console.WriteLine(string.Format("Seat Assignment file has been saved at {0}", outputFilePath));
            }
            catch (Exception ex)
            {
                //TODO: Handle Exceptions
                Console.WriteLine("Something went wrong");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine("Aborting...");
            }
            finally
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
