using System.Web.Mvc;

namespace CRM_HELIANTIS.Areas.Projet
{
    public class ProjetAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Projet";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Projet_default",
                "Projet/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
