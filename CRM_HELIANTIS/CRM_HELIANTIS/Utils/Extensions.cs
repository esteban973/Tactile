#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Mvc;
#endregion

namespace CRM_HELIANTIS.Utils
{
    public static class Extensions
    {
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                0,
                dateTime.Kind);
        }

        public static DateTime ChangeTimeString(this DateTime dateTime, string temps)
        {
            return dateTime.ChangeTime(Int32.Parse(temps.Substring(0, 2)), Int32.Parse(temps.Substring(3, 2)));
        }

        public static DateTime TimeUnix(this double temps)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return date.AddSeconds(temps);
        }

        public static IEnumerable ListeErreurs(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return (from item in modelState.Values
                from error in item.Errors
                select error.ErrorMessage).ToList();
            }
            return null;
        }

    }
} 