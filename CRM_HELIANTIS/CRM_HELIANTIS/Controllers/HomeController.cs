using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;

namespace CRM_HELIANTIS.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (this.isConnected()) return RedirectToAction("index", "Planning", new { area = "Planning" });
            else return RedirectToAction("Index", "Connexion", new { area = "Utilisateur" });
        }

    }
}
