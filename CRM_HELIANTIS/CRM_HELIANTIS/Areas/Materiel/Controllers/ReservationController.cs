using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Models;
using CRM_HELIANTIS.Areas.Materiel.Models;
using System.Data;
using CRM_HELIANTIS.Utils;

namespace CRM_HELIANTIS.Areas.Materiel.Controllers
{
    public class ReservationController : BaseController
    {
        

        public ActionResult Index()
        {
            ViewBag.materiel=new SelectList(cnx.materiel, "id_materiel", "nom");
            return View("Reservation");
        }

        public JsonResult ListeResa(int idMatos, double start, double end)
        {
            var debut=start.TimeUnix();
            var fin = end.TimeUnix();
            var resas = cnx.reservation.Where(r => r.id_materiel == idMatos && r.date_debut > debut && r.date_debut < fin && r.employe.z_actif==true).ToList();
            List<object> retour=new List<object>();
            foreach (var resa in resas){
                retour.Add(resa.PourJson());
                     }
            return Json(retour);
        }

        public JsonResult Supprimer(int id)
        {
            try
            {
                var resa = cnx.reservation.Where(r => r.id_reservation == id).Single();
                cnx.DeleteObject(resa);
                cnx.SaveChanges();
                return Json(new { succes = 1 });
            } catch (ArgumentNullException){
                return Json(new { succes = 0 });
            }
            
        }


        public JsonResult Commenter(int id, string commentaires)
        {
            try
            {
                var resa = cnx.reservation.Where(r => r.id_reservation == id).Single();
                resa.commentaires = commentaires;
                cnx.ObjectStateManager.ChangeObjectState(resa, EntityState.Modified);
                cnx.SaveChanges();
                return Json(new { succes = 1 });
            }
            catch (ArgumentNullException)
            {
                return Json(new { succes = 0 });
            }

        }

        public JsonResult Editer(int? id, ReservationView resaview)
        {
            if (ModelState.IsValid)
            {
                reservation resa;
                object retour;
                var resas = cnx.reservation.Where(r => r.id_materiel == resaview.id_matos && r.date_fin > resaview.debut && r.date_debut < resaview.fin && r.employe.z_actif == true);
                if (id.HasValue)
                {
                    resas = resas.Where(r => r.id_reservation != id);
                }
                if (resas.Any()) return Json(new { succes = 0, message = "La ressource n'est pas disponible." });
                if (!id.HasValue)
                {
                    resa = new reservation();
                    resa.UpdateFromModelView(resaview);
                    
                    cnx.AddToreservation(resa);
                    cnx.SaveChanges();
                    retour = new { succes = 1, creation=1, resa = resa.PourJson() };
                } else {
                    resa=cnx.reservation.Where(r=>r.id_reservation==id).Single();
                    resa.UpdateFromModelView(resaview);
                    cnx.ObjectStateManager.ChangeObjectState(resa, EntityState.Modified);
                    cnx.SaveChanges();
                    retour = new { succes = 1, creation = 0 };
                }
                
                return Json(retour);
            }
            else
            {
                return Json(new { succes = 0, message = "Une erreur s'est produite" });
            }
        }



        #region Gestion de la duplication d'un évenement

        [HttpGet]
        [RulesManager("access")]
        public PartialViewResult Dupliquer(int id)
        {
            reservation resa = cnx.reservation.Where(r => r.id_reservation == id).Single();
            DupliquerView dpv = new DupliquerView();
            dpv.dateFin = resa.date_debut.AddDays(1);
            dpv.id_resa = id;
            return PartialView("_formDupliquer", dpv);
        }

        [HttpPost]
        [RulesManager("access")]
        public JsonResult Dupliquer(DupliquerView duplicate)
        {
            if (ModelState.IsValid)
            {
                reservation resa = cnx.reservation.Where(r => r.id_reservation == duplicate.id_resa).Single();
                recurrence rec = new recurrence();
                resa.recurrence = rec;
                DateTime debut = resa.date_debut;
                DateTime fin = resa.date_fin;
                int diffDays = duplicate.frequenceInt;
                while (debut.AddDays(diffDays) < duplicate.dateFin.AddDays(1))
                {
                    reservation resaCopie = resa.ClonerPourDupliquer();
                    debut = debut.AddDays(diffDays);
                    fin = fin.AddDays(diffDays);
                    resaCopie.date_debut = debut;
                    resaCopie.date_fin = fin;
                    //verification si on le garde ou pas
                    // si c'est un samedi ou un dimanche
                    if ((debut.DayOfWeek == DayOfWeek.Saturday) || (debut.DayOfWeek == DayOfWeek.Sunday) ) continue;
                    if (cnx.reservation.Where(r => r.id_materiel == resaCopie.id_materiel && r.date_fin > resaCopie.date_debut && r.date_debut < resaCopie.date_fin && r.employe.z_actif == true).Any() == false)
                    {
                        resaCopie.recurrence = rec;
                        cnx.AddToreservation(resaCopie);
                    }
                }
                cnx.SaveChanges();
                return Json(new { succes = 1 });
            }
            else
            {
                return Json(new { succes = 0, erreurs = ModelState.ListeErreurs() });
            }
        }

        [HttpPost]
        [RulesManager("access")]
        public JsonResult SupprimerRecurrence(int id)
        {
            var resas = cnx.reservation.Where(r => r.id_recurrence == id).ToList();
            foreach (var resa in resas)
            {
                cnx.DeleteObject(resa);
            }
            var rec = cnx.recurrence.Where(r => r.id_recurrence == id).Single();
            cnx.DeleteObject(rec);
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }

        [HttpPost]
        [RulesManager("access")]
        public JsonResult SupprimerUlterieur(int id)
        {
            reservation resa = cnx.reservation.Where(r => r.id_reservation == id).Single();
            var resas = cnx.reservation.Where(r => r.date_debut >= resa.date_debut && r.id_recurrence == resa.id_recurrence).ToList();
            foreach (var r in resas)
            {
                cnx.DeleteObject(r);
            }
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }


        #endregion


    }
}
