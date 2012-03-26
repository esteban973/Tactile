using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace CRM_HELIANTIS.Utils
{
    public static class PropertiesExtensions
    {
        /// <summary>
        /// Retourne une propriété par son nom
        /// </summary>
        /// <param name="MClass"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static dynamic GetByName(this object MClass, string name)
        {
            Type t = MClass.GetType();
            PropertyInfo p = t.GetProperty(name);

            return p.GetValue(MClass, null);
        }


    }
}