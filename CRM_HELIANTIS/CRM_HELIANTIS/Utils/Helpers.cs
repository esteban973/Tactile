using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_HELIANTIS.Utils
{
    public static class UtilsHelpers
    {
        #region retourne si un contrôleur dans un Area existe 
        public static bool AreaControllerExist(this System.Web.Mvc.HtmlHelper helper, string AreaName)
        {
            return UtilsHelpers.AreaControllerExist(helper, AreaName, "");
        }

        public static bool AreaControllerExist(this System.Web.Mvc.HtmlHelper helper, string AreaName, string AssemblyName)
        {
            string asmName = AssemblyName;
            if ( string.IsNullOrEmpty( asmName ))
                 asmName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            Type type = Type.GetType(asmName + ".Areas." + AreaName + ".Controllers.BackofficeController, " + asmName);

            if (type != null)
                return true;
            else
                return false;
        }
        #endregion

        public static string ConvertToFrench(this HtmlHelper ctrl, string JourSemaine)
        {
            switch (JourSemaine)
            {
                case ("Monday"): return "Lundi";
                case ("Tuesday"): return "Mardi";
                case ("Wednesday"): return "Mercredi";
                case ("Thursday"): return "Jeudi";
                case ("Friday"): return "Vendredi";
                case ("Saturday"): return "Samedi";
                case ("Sunday"): return "Dimanche";
                default: return JourSemaine;
            }
            
        }

    }
}