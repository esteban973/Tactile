using System.Web.Mvc;

namespace CRM_HELIANTIS.Areas.Utilisateur
{
    public class UtilisateurAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Utilisateur";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Utilisateur_default",
                "Utilisateur/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
