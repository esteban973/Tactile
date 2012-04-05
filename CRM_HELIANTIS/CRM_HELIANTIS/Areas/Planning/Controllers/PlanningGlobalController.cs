using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Models;
using CRM_HELIANTIS.Areas.Planning.Models;

namespace CRM_HELIANTIS.Areas.Planning.Controllers
{
    public class PlanningGlobalController : BaseController
    {
        //
        // GET: /Planning/PlanningGlobal/
        [RulesManager("access")]
        public ActionResult Index()
        {
            ViewBag.type_temps = cnx.type_temps.ToList();
            ViewBag.serviceList = new SelectList(cnx.groupe, "id_groupe", "nom", this.getCurrentUtilisateur().id_groupe);
            return View();
        }

        [RulesManager("access")]
        public JsonResult ListeJournees(DateTime dateJour, int? cat)
        {
            // on fait appel à la procédure stockées qui va me renvoyer des objets de type VuePlanningGlobal
            List<VuePlanningGlobal> detailstemps = cnx.spVuePlanningGlobal(dateJour.AddDays(-15), dateJour.AddDays(45)).ToList();
            // j'instancie l'objet reponse
            List<Object> reponse = new List<object>();
            // l'affichage des employés peut être conditionné en fonction du service
            // si l'entier cat est renseigné on limite la liste des employés à seulement le groupe correspondant
            List<employe> listEmp;
            if (cat.HasValue) listEmp = cnx.employe.Where(e => e.id_groupe == cat && e.z_actif == true).OrderBy(e=>e.nom).ToList();
            else listEmp = cnx.employe.Where(e => e.z_actif == true).OrderBy(e => e.nom).ToList();
            
            foreach (var emp in listEmp)
            {
                // maintenant pour chaque employé on va construire un objet ligneGantt 
                // qui correspondra à une ligne du tableau de Gantt
                LigneGantt ligne = new LigneGantt(emp);
                // dans l'objet dteailsTemps on va pouvoir faire une requete 
                //à la recherche de tous les temps de l'employé
                var temps = detailstemps.Where(t => t.idemploye == emp.id_employe).ToList();
                foreach (var tps in temps)
                {
                   // pour chaque temps, je vais ajouter à l'objet LigneGantt qui va en faire le traitement
                    ligne.ajouteAJourEmps(tps);
                }
                reponse.Add(ligne.retourEnJson());
            }
            //creation d'un événement de la durée du tableau pour avoir la bonne taille du tableau
            List<object> liste = new List<object>();
            liste.Add(new { from = dateJour.AddDays(-15), to = dateJour.AddDays(45) });
            reponse.Add(new { name = "", desc = "", values = liste });
            return Json(reponse, JsonRequestBehavior.AllowGet);
        }

    }
}
