using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM_HELIANTIS.Global
{
    public static  class Constants
    {
        public enum orderByEnum
        {
            asc=0
            ,desc=1
        }

        public static string WEBSITEROOT = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;

        public const decimal BASEFRAISKILOMETRIK = 0.45m ;

    } // fin classe

} // fin namespace