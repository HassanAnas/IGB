using System;

namespace IGB.Services
{
    public class TimeConversionService
    {
        public DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId)
        {
            try
            {
                // Find the time zone
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                // Convert to UTC
                DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZoneInfo);
                return utcDateTime;
            }
            catch (TimeZoneNotFoundException)
            {
                throw new ArgumentException("The specified time zone ID was not found.");
            }

        }


        public DateTime ConvertFromUtc(DateTime utcDateTime, string timeZoneId)
        {
            try
            {
                // Find the time zone
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                // Convert from UTC to the specified time zone
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
                return localDateTime;
            }
            catch (TimeZoneNotFoundException)
            {
                throw new ArgumentException("The specified time zone ID was not found.");
            }
        }
    }
}