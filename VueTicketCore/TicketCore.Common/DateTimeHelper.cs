using System;
using System.Collections.Generic;
using System.Globalization;

namespace TicketCore.Common
{
    public class DateTimeHelper
    {
        public DateTimeHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// This method will take a DateTime string as the first parameter and the format
        /// of the DateTime string as the second parameter. It will then convert the string to DateTime

        /// object and return the newly create object
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="stringFormat"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string dateTimeString, string stringFormat)
        {
            if (dateTimeString != "" && stringFormat != "")
            {
                try
                {
                    DateTime result = DateTime.ParseExact(dateTimeString, stringFormat, new CultureInfo("en-GB"));
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                throw new Exception("Both parameters must have value (dateTimeString, stringFormat)!");
            }
        }

        /// <summary>
        /// This method will return an integer representing the number of days between two
        /// particular dates passed to a method
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDaysDifference(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDifference = endDate - startDate;

            try
            {
                return Int32.Parse(timeDifference.TotalDays.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// This method will return an integer representing the number of months between two
        /// particular dates passed to a method
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetMonthsCountBetweenDates(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDifference = endDate - startDate;

            DateTime resultDate = DateTime.MinValue + timeDifference;

            int monthDifference = resultDate.Month - 1;

            return monthDifference;
        }

        /// <summary>
        /// This method will take 2 dates as input and return the Generic List of DateTime objects. Each
        /// object represent 1 of the months between the 2 dates and is set in the dd/MM/yyyy format while
        /// the current day of the month is set to 1. For example: 01/01/2010, 01/02/2010,01/03/2010 etc..
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<DateTime> GetMonthsBetweenDates(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDifference = endDate - startDate;

            DateTime resultDate = DateTime.MinValue + timeDifference;

            int monthDifference = resultDate.Month - 1;

            if (monthDifference > 0)
            {
                List<DateTime> result = new List<DateTime>();
                for (int i = 0; i < monthDifference; i++)
                {
                    DateTime tempDate = startDate.AddMonths(1);
                    DateTime dateToAdd = new DateTime(tempDate.Date.Year, tempDate.Month, 1);

                    result.Add(dateToAdd);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an indication whether the year passed as method parameter is a leap year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        /// This method returns a string array of week days which can be used for DropDownLists and such
    /// </summary>
    /// <returns></returns>
        public static string[] WeekDaysAsStringArray()
        {
            string[] result = new string[7];

            result[0] = "Monday";
            result[1] = "Tuesday";
            result[2] = "Wednesday";
            result[3] = "Thursday";
            result[4] = "Friday";
            result[5] = "Saturday";
            result[6] = "Sunday";

            return result;
        }

      

    }
}