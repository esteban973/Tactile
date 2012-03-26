using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Models;
using System.Data;
using CRM_HELIANTIS.Areas.Projet.Models;
using CRM_HELIANTIS.Utils;

namespace CRM_HELIANTIS.Areas.Projet.Controllers
{
    public class ProjetController : BaseController
    {
        //
        // GET: /Projet/Client/
        [RulesManager("can_manage_projets")]
        public ActionResult Index()
        {
            return View();
        }

        [RulesManager("can_manage_projets")]
        public JsonResult listeProjet()
        {
            var projets = cnx.projet.Where(p=>p.z_actif==true).ToList();
            List<object> retour = new List<object>();
            foreach (var projet in projets)
            {
                retour.Add(new
                {
                    id = projet.id_projet,
                    reference = projet.reference,
                    nom = projet.nom,
                    client =projet.client.nom,
                    bouton = "<span class='edit' data-id=" + projet.id_projet + ">Editer</span><span class='suppr' data-identite='" + projet.nom + "' data-id=" + projet.id_projet + ">Supprimer</span>"
                });
            }
            return Json(retour, JsonRequestBehavior.AllowGet);
        }

        [RulesManager("can_manage_projets")]
        public PartialViewResult Creer()
        {
            ViewBag.type = "creer";
            ProjetView projetview = new ProjetView();
            var clients = cnx.client.Where(c => c.z_actif == true);
            ViewBag.id_client = new SelectList(clients, "id_client", "nom");
            return PartialView("_form", projetview);
        }


        [RulesManager("can_manage_projets")]
        [HttpPost]
        public ActionResult Editer(ProjetView projetview, int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    projet projetx = new projet();
                    projetx.updateFromModel(projetview);
                    cnx.projet.AddObject(projetx);
                }
                else {
                    projet projetx = cnx.projet.Where(p => p.id_projet == id).Single();
                    projetx.updateFromModel(projetview);
                    cnx.ObjectStateManager.ChangeObjectState(projetx, EntityState.Modified);
                }
                    cnx.SaveChanges();
                    return Json(new { succes = 1 });
            }
            return Json(new { succes = 0, erreurs = ModelState.ListeErreurs() });
        }

        //
        //
        [RulesManager("can_manage_projets")]
        public PartialViewResult Editer(int id)
        {
            ViewBag.type = "editer";
            projet projet = cnx.projet.Single(p => p.id_projet == id);
            ProjetView projetview = new ProjetView();
            projetview.updateFromModel(projet);
            var clients = cnx.client.Where(c => c.z_actif == true).ToList();
            ViewBag.id_client = new SelectList(clients, "id_client", "nom", projetview.id_client);
            return PartialView("_form", projetview);
        }

        //
        //TODO gérer la cascade
        [RulesManager("can_manage_projets")]
        [HttpPost]
        public ActionResult Effacer(int id)
        {
            var projetx = cnx.projet.Where(p => p.id_projet == id).Single();
            projetx.z_actif = false;
            cnx.ObjectStateManager.ChangeObjectState(projetx, EntityState.Modified);
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }

    }
}
