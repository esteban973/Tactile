using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Models;
using System.Data;
using CRM_HELIANTIS.Areas.Materiel.Models;
using CRM_HELIANTIS.Utils;

namespace CRM_HELIANTIS.Areas.Materiel.Controllers
{
    public class MaterielController : BaseController
    {
        //
        // GET: /Projet/Client/
        [RulesManager("can_manage_materiels")]
        public ActionResult Index()
        {
            return View();
        }

        [RulesManager("can_manage_materiels")]
        public JsonResult listeMateriel()
        {
            var materiels = cnx.materiel.ToList();
            List<object> retour = new List<object>();
            foreach (var materiel in materiels)
            {
                retour.Add(new
                {
                    id = materiel.id_materiel,
                    nom = materiel.nom,
                    bouton = "<span class='edit' data-id=" + materiel.id_materiel + ">Editer</span><span class='suppr' data-identite='" + materiel.nom + "' data-id=" + materiel.id_materiel + ">Supprimer</span>"
                });
            }
            return Json(retour, JsonRequestBehavior.AllowGet);
        }

        [RulesManager("can_manage_materiels")]
        public PartialViewResult Creer()
        {
            ViewBag.type = "creer";
            MaterielView materielview = new MaterielView();
            return PartialView("_form", materielview);
        }


        [RulesManager("can_manage_materiels")]
        [HttpPost]
        public ActionResult Editer(MaterielView materielview, int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    materiel materielx = new materiel();
                    materielx.updateFromModel(materielview);
                    cnx.materiel.AddObject(materielx);
                }
                else
                {
                    materiel materielx = cnx.materiel.Where(m => m.id_materiel == id).Single();
                    materielx.updateFromModel(materielview);
                    cnx.ObjectStateManager.ChangeObjectState(materielx, EntityState.Modified);
                }
                cnx.SaveChanges();
                return Json(new { succes = 1 });
            }
            return Json(new { succes = 0, erreurs = ModelState.ListeErreurs() });
        }

        //
        //
        [RulesManager("can_manage_materiels")]
        public PartialViewResult Editer(int id)
        {
            ViewBag.type = "editer";
            materiel materiel = cnx.materiel.Single(m => m.id_materiel == id);
            MaterielView materielview = new MaterielView();
            materielview.updateFromModel(materiel);
            return PartialView("_form", materielview);
        }



        [RulesManager("can_manage_materiels")]
        [HttpPost]
        public ActionResult Effacer(int id)
        {

            var materiel = cnx.materiel.Where(m => m.id_materiel == id).Single();
            cnx.materiel.DeleteObject(materiel);
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }

    }

}

