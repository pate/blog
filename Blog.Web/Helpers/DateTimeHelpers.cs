using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class DateTimeHelpers
    {
        public static string TimeSince(this DateTime dt)
        {
            var ts = new TimeSpan(System.DateTime.Now.Ticks - dt.Ticks);

            double delta = ts.TotalSeconds;

            const int SECOND = 1;
            const int MINUTE = 60*SECOND;
            const int HOUR = 60*MINUTE;
            const int DAY = 24*HOUR;
            const int MONTH = 30*DAY;

            /*if (delta < 3 * SECOND) // took it out because "just now ago" looks lame
            {
                return "just now";
            }*/
            if (delta < 1*MINUTE)
            {
                return ts.Seconds == 1 ? "one second" : ts.Seconds + " seconds";
            }
            if (delta < 2*MINUTE)
            {
                return "1 minute";
            }
            if (delta < 45*MINUTE)
            {
                return ts.Minutes + " minutes";
            }
            if (delta < 90*MINUTE)
            {
                return "1 hour";
            }
            if (delta < 24*HOUR)
            {
                return ts.Hours == 1 ? "1 hour" : ts.Hours + " hours";
            }
            if (delta < 48*HOUR)
            {
                return "1 day";
            }
            if (delta < 30*DAY)
            {
                return ts.Days + " days";
            }
            if (delta < 12*MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double) ts.Days/30));
                return months <= 1 ? "1 month" : months + " months";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double) ts.Days/365));
                return years <= 1 ? "1 year" : years + " years";
            }
        }
    }

}

namespace iFix.Helpers
{
	public class DateFormat
	{
		public static string Short
		{
			get
			{
				return "{0:yyyy-MM-dd}";
			}
		}
		public static string Universal
		{
			get
			{
				return "{0:u}";
			}
		}
		public static string Sortable
		{
			get
			{
				return "{0:s}";
			}
		}
		public static string Readable
		{
			get
			{
				return "{0:d MMMM yyyy}";
			}
		}

		public static string DateAtTime
		{
			get
			{
				return "{0:d MMM} at {0:t}";
			}
		}
	}
}