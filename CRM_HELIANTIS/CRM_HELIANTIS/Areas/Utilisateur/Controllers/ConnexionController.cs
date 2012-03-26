using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using System.Data;
using CRM_HELIANTIS.Areas.Utilisateur.Models;

namespace CRM_HELIANTIS.Areas.Utilisateur.Controllers
{
    public class ConnexionController : BaseController
    {
        

        public ActionResult Index()
        {
            return View("Connexion");
        }


        public ActionResult SeConnecter(FormCollection form)
        {
            this.rulesManager.setConnexion(form["identifiant"], form["password"]);
            if (!this.isConnected())
            {
                ViewBag.message = "Le mot de passe et l'identifiant ne correspondent pas";
                return View("Connexion");
            }
            else
            {
                return this.RedirectToAction("index", "Planning", new { area = "Planning" });
            }
        }

        public ActionResult SeDeconnecter()
        {
            this.rulesManager.Disconnected();
            return this.RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult MotPassePerdu(string email)
        {
            var employe = cnx.employe.Where(e => e.email == email).FirstOrDefault();
            if (employe == null) return Json(new { succes = 0, message = "Aucun utilisateur ne correspond à l'email que vous avez renseigné" });
                employe.utilisateur.mot_passe = this.RandomString();
                cnx.ObjectStateManager.ChangeObjectState(employe, EntityState.Modified);
                cnx.SaveChanges();
                EmailInfoConnexion infoEmail = new EmailInfoConnexion(employe);
                new MailController().MotPassePerdu(infoEmail).Deliver();
                return Json(new { succes = 1, message = "Un nouveau mot de passe vient de vous être envoyé." });
                        
        }


    }
}
