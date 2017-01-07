using System;
using System.Configuration;

namespace SeatAssignment.Entities
{
    /// <summary>
    /// Reads and stores configuration from the config file. 
    /// Prevents multiple visits to the config file every time it is requested
    /// </summary>
    public static class ConfigurationReader
    {
        private static int _numberOfRows;

        public static int NumberOfRows
        {
            get
            {
                if (_numberOfRows != 0)
                    return _numberOfRows;
                else
                {
                    var appSetting = ConfigurationManager.AppSettings["NumberOfRows"];
                    var value = Convert.ToInt32(appSetting);
                    if (value <= 0)
                        throw new ArgumentOutOfRangeException("NumberOfRows", "Value must be positive");
                    _numberOfRows = value;
                    return _numberOfRows;
                }
            }
        }

        private static int _seatsInEachRow;

        public static int SeatsInEachRow
        {
            get
            {
                if (_seatsInEachRow != 0)
                    return _seatsInEachRow;
                else
                {
                    var appSetting = ConfigurationManager.AppSettings["SeatsInEachRow"];
                    var value = Convert.ToInt32(appSetting);
                    if (value <= 0)
                        throw new ArgumentOutOfRangeException("SeatsInEachRow", "Value must be positive");
                    _seatsInEachRow = value;
                    return _seatsInEachRow;
                }
            }
        }

        private static string _defaultInputFilePath;

        public static string DefaultInputFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultInputFilePath))
                {
                    _defaultInputFilePath = ConfigurationManager.AppSettings["DefaultInputFilePath"];
                    return _defaultInputFilePath;
                }
                else
                {
                    return _defaultInputFilePath;
                }
            }
        }

        private static string _defaultOutputFilePath;

        public static string DefaultOutputFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultOutputFilePath))
                {
                    _defaultOutputFilePath = ConfigurationManager.AppSettings["DefaultOutputFilePath"];
                    return _defaultOutputFilePath;
                }
                else
                {
                    return _defaultOutputFilePath;
                }
            }
        }

    }
}
