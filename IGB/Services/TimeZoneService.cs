using System;
using System.Collections.Generic;
using System.Linq;

namespace IGB.Services
{
    public class TimeZoneService
    {
        public List<TimeZoneInfo> GetAllTimeZones()
        {
            try
            {
                return TimeZoneInfo.GetSystemTimeZones().ToList();
            }
            catch (Exception)
            {

                throw;
            }  
        }
    }
}

