using System;
using System.Configuration;

namespace SeatAssignment.Entities
{
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
                    var appSetting = ConfigurationManager.AppSettings["NumberofRows"];
                    _numberOfRows = Convert.ToInt32(appSetting);
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
                    _seatsInEachRow = Convert.ToInt32(appSetting);
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
