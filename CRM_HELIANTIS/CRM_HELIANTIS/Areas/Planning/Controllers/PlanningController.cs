using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Areas.Planning.Models;
using CRM_HELIANTIS.Models;
using System.Data;
using CRM_HELIANTIS.Utils;

namespace CRM_HELIANTIS.Areas.Planning.Controllers
{
    public class PlanningController : BaseController
    {


        #region Gestion de l'affichage du Planning
        [RulesManager("access")]
        public ActionResult Index()
        {
            var employes = from e in cnx.employe
                           where (e.z_actif == true)
                           select new { nomComplet = e.prenom + " " + e.nom.ToUpper(), id_employe = e.id_employe };
            ViewBag.employeList = new SelectList(employes, "id_employe", "nomComplet", this.getCurrentUtilisateur().id_employe);
            ViewBag.absenceList = cnx.type_temps.Where(t => t.absence == true).ToList();
            ViewBag.travailList = cnx.type_temps.Where(t => t.absence == false).ToList();
            return View("Planning");
        }

        [RulesManager("access")]
        public JsonResult ListeTemps(int idEmploye, double start, double end)
        {
            var debut = start.TimeUnix();
            var fin = end.TimeUnix();
            var temps = cnx.temps.Where(t => t.id_employe == idEmploye && t.date_debut > debut && t.date_debut < fin && (t.projet.z_actif == true || t.projet == null)).ToList();
            List<object> retour = new List<object>();
            foreach (var temp in temps)
            {
                retour.Add(temp.PourJson());
            }
            return Json(retour);
        }

        [RulesManager("access")]
        public JsonResult HorairesEmploye(int id)
        {
            employe emp = cnx.employe.Where(e => e.id_employe == id).Single();
            return Json(new { debMat = emp.horaires_matin, finMat = emp.horaires_matin_fin, debAm = emp.horaires_apresmidi, finAm = emp.horaires_apresmidi_fin }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Gestion des formulaires d'ajout et d'éditionEditionFormulaire
        [RulesManager("access")]
        public PartialViewResult AjoutFormulaire(DateTime debut, DateTime fin, int id_employe)
        {

            ViewBag.debut = debut;
            ViewBag.fin = fin;
            ViewBag.id_employe = id_employe;
            return PartialView("_form");
        }
        
        [RulesManager("access")]
        public PartialViewResult EditionFormulaire(int id, bool absence)
        {

            ViewBag.absence = absence;
            ViewBag.editer = true;
            ViewBag.id_employe = id;
            return PartialView("_form");
        }

        [RulesManager("access")]
        public PartialViewResult AjoutFormulaireAbsence(DateTime debut, DateTime fin, int id_employe)
        {
            var typesAbsences = from absence in cnx.type_temps
                                where (absence.absence == true)
                                select new { nomAbsence = absence.nom, id_type_absence = absence.id_type_temps };
            ViewBag.type_absence = new SelectList(typesAbsences, "id_type_absence", "nomAbsence");
            AbsenceView absenceview = new AbsenceView();
            absenceview.id_employe = id_employe;
            absenceview.dateAbsence = debut;
            absenceview.debutAbsence = debut.ToShortTimeString();
            absenceview.finAbsence = fin.ToShortTimeString();
            return PartialView("_formAbsence", absenceview);
        }

        [RulesManager("access")]
        public PartialViewResult EditionFormulaireAbsence(int id)
        {

            temps tps = cnx.temps.Where(t => t.id_temps == id).Single();
            var typesAbsences = from absence in cnx.type_temps
                                where (absence.absence == true)
                                select new { nomAbsence = absence.nom, id_type_absence = absence.id_type_temps };
            ViewBag.type_absence = new SelectList(typesAbsences, "id_type_absence", "nomAbsence", tps.id_type_temps);
            AbsenceView absenceview = new AbsenceView();
            absenceview.UpdateFromModel(tps);
            ViewBag.type = "editer";
            return PartialView("_formAbsence", absenceview);
        }

        [RulesManager("access")]
        public PartialViewResult AjoutFormulaireTravail(DateTime debut, DateTime fin, int id_employe)
        {
            var typesTravail = from travail in cnx.type_temps
                               where (travail.absence == false)
                               select new { nomTravail = travail.nom, id_type_travail = travail.id_type_temps };
            ViewBag.type_travail = new SelectList(typesTravail, "id_type_travail", "nomTravail", 2);

            var projets = from p in cnx.projet
                          where ( p.z_actif == true)
                          select new { nomProjet = p.reference + " " + p.nom + " / " + p.client.nom, id_projet = p.id_projet };
            ViewBag.id_projet = new SelectList(projets, "id_projet", "nomProjet");

            var emp = cnx.employe.Where(e => e.id_employe == id_employe).Single();

            var taches = from task in cnx.tache
                         select new { id_tache = task.id_tache, nom_tache = task.nom_tache };
            ViewBag.id_tache = new SelectList(taches, "id_tache", "nom_tache", emp.id_tache);

            TravailView travailview = new TravailView();
            travailview.id_employe = id_employe;
            travailview.dateTravail = debut;
            travailview.debutTravail = debut.ToShortTimeString();
            travailview.finTravail = fin.ToShortTimeString();
            return PartialView("_formTravail", travailview);
        }

        [RulesManager("access")]
        public PartialViewResult EditionFormulaireTravail(int id)
        {

            temps tps = cnx.temps.Where(t => t.id_temps == id).Single();
            var typesTravail = from travail in cnx.type_temps
                               where (travail.absence == false)
                               select new { nomTravail = travail.nom, id_type_travail = travail.id_type_temps };
            ViewBag.type_travail = new SelectList(typesTravail, "id_type_travail", "nomTravail", tps.id_type_temps);

            var projets = from p in cnx.projet
                          where ( p.z_actif == true)
                          select new { nomProjet = p.reference + " " + p.nom + " / " + p.client.nom, id_projet = p.id_projet };
            ViewBag.id_projet = new SelectList(projets, "id_projet", "nomProjet", tps.id_projet);

            ViewBag.id_tache = new SelectList(cnx.tache, "id_tache", "nom_tache", tps.id_tache);
            TravailView travailview = new TravailView();
            travailview.UpdateFromModel(tps);
            ViewBag.type = "editer";
            return PartialView("_formTravail", travailview);
        }

        #endregion


        #region Enregistrement des ajouts, éditions et suppression des temps

        [HttpPost]
        [RulesManager("access")]
        public JsonResult EnregistrerAbsence(int? id, AbsenceView absenceview)
        {
            if (ModelState.IsValid)
            {
                if (!id.HasValue)
                {
                    temps absence = new temps();
                    absence.UpdateFromView(absenceview);
                    var employe = cnx.employe.Where(e => e.id_employe == absence.id_employe).Single();
                    absence.CorrigerEnDemiJournee(employe.horaires_matin, employe.horaires_matin_fin, employe.horaires_apresmidi, employe.horaires_apresmidi_fin);
                    // s'assurer qu'il n'y a pas d'autres congés en même temps sinon lancer une erreur
                    if (this.IsThereAchevauchement(absence, true) == false)
                    {
                        cnx.AddTotemps(absence);
                        cnx.SaveChanges();
                        return Json(new { succes = 1, creation = 1, temps = absence.PourJson() });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Les dates ne conviennent pas. (superposition d'absence et de travail) ");
                    }
                }
                else
                {
                    temps absence = cnx.temps.Where(t => t.id_temps == id).Single();
                    absence.UpdateFromView(absenceview);
                    // va faire coller l'absence à exactement à une demi journée en comprant avec la date de début de l'évenement
                    absence.CorrigerEnDemiJournee(absence.employe.horaires_matin, absence.employe.horaires_matin_fin, absence.employe.horaires_apresmidi, absence.employe.horaires_apresmidi_fin);
                    // dans le cas d'une modification d'un évenement de travail en congé s'assurer que tous les frais ont bien été enlevé
                    if (absence.id_frais.HasValue)
                    {
                        cnx.DeleteObject(absence.frais);
                        absence.id_frais = null;
                    }
                    // s'assurer qu'il n'y a pas d'autres congés en même temps sinon lancer une erreur
                    if (this.IsThereAchevauchement(absence, true) == false)
                    {
                        cnx.ObjectStateManager.ChangeObjectState(absence, EntityState.Modified);
                        cnx.SaveChanges();
                        return Json(new { succes = 1 });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Les dates ne conviennent pas. (superposition d'absence et de travail) ");
                    }

                }
            }
            return Json(new { succes = 0, erreurs = ModelState.ListeErreurs() });

        }



        [HttpPost]
        [RulesManager("access")]
        public JsonResult EnregistrerTravail(int? id, TravailView travailview)
        {
            //On utilise les annotations pour vérifier la validité de l'objet
            if (ModelState.IsValid)
            {
                // on teste l'id envoyé dans l'URL. S'il n'y en a pas, c'est que cela est un nouveau temps. 
                if (id == null)
                {
                    // on instancie le nouveau temps et le met en jour grâce aux données de la vue du formulaire
                    temps travail = new temps();
                    travail.UpdateFromView(travailview);
                    // si le temps a des frais on instancie un nouvel objet frais lié à ce temps
                    if (travailview.hasFrais())
                    {
                        frais frs = new frais();
                        frs.updateFromView(travailview);
                        travail.frais = frs;
                        cnx.AddTofrais(frs);
                    }
                    // s'assurer qu'il n'y a pas de congés en même temps sinon on rajoute une erreur au ModelState
                    if (this.IsThereAchevauchement(travail, false) == false)
                    {
                        // on rajoute à l'EntityManager l'objet créé et on sauvegarde
                        cnx.AddTotemps(travail);
                        cnx.SaveChanges();
                        // on renvoie un objet anonyme précisant si cela a fonctionné, si c'est une création
                        // et une version de l'objet travail pour qu'il soit intégré par le plugin FullCalendar
                        return Json(new { succes = 1, creation = 1, temps = travail.PourJson() });
                    }
                    else
                    {
                        //  on rajoute une erreur à l'objet ModelState
                        ModelState.AddModelError("", "Les dates ne conviennent pas. (superposition d'absence et de travail) ");
                    }

                }
                else
                {
                    // dans le cas d'une modification on recherche le temps concerné par l'id
                    temps travail = cnx.temps.Where(t => t.id_temps == id).Single();
                    // on le met en jour grâce aux données de la vue du formulaire
                    travail.UpdateFromView(travailview);
                    // on recherche les frais associés à ce temps
                    frais frs = travail.frais;
                    // s'il n'y en pas
                    if (frs != null)
                    {
                        // on vérifie que le formulaire a les champs frais renseignés
                        if (travailview.hasFrais())
                        {
                            // si c'est le cas, on met à jour l'objet frais associé et on le sauvegarde
                            frs.updateFromView(travailview);
                            cnx.ObjectStateManager.ChangeObjectState(frs, EntityState.Modified);
                        }
                        else
                        {
                            // on supprime l'objet associé
                            cnx.DeleteObject(frs);
                           travail.frais = null;
                        }
                    }
                    else
                    {   
                        // si le travail n'avait pas frasi et que le formuakire en a
                        if (travailview.hasFrais())
                        {
                            // on crée l'objet frais, on l'associe à Temps et on sauvegarde
                            frs = new frais();
                            frs.updateFromView(travailview);
                            travail.frais = frs;
                            cnx.AddTofrais(frs);
                        }
                    }

                    // s'assurer qu'il n'y a pas de congés en même temps sinon lancer une erreur
                    if (this.IsThereAchevauchement(travail, false) == false)
                    {
                       // on enregsitre l'objet modifié et on renvoie l'objet anonyme en Json précisant le succes
                        cnx.ObjectStateManager.ChangeObjectState(travail, EntityState.Modified);
                        cnx.SaveChanges();
                        return Json(new { succes = 1 });
                    }
                    else
                    {
                        //  on rajoute une erreur à l'objet ModelState
                        ModelState.AddModelError("", "Les dates ne conviennent pas. (superposition d'absence et de travail) ");
                    }

                }
            }
            // on renvoie un objet anonyme indiquant que l'objet n'a pas été enregistré correctement
            // et on renvoie la liste des erreurs.
            return Json(new { succes = 0, erreurs = ModelState.ListeErreurs() });
        }



        [HttpPost]
        [RulesManager("access")]
        public JsonResult ModifierDates(int id, DateTime debut, DateTime fin)
        {
            temps tps = cnx.temps.Where(t => t.id_temps == id).Single();
            tps.date_debut = debut;
            tps.date_fin = fin;
            bool tempss;
            if (tps.type_temps.absence == true)
            {
                tps.CorrigerEnDemiJournee(tps.employe.horaires_matin, tps.employe.horaires_matin_fin, tps.employe.horaires_apresmidi, tps.employe.horaires_apresmidi_fin);
                tempss = this.IsThereAchevauchement(tps, true);
            }
            else
            {
                tempss = this.IsThereAchevauchement(tps, false);
            }
            if (tempss == false)
            {
                cnx.ObjectStateManager.ChangeObjectState(tps, EntityState.Modified);
                cnx.SaveChanges();
                return Json(new { succes = 1 });
            }
            else
            {
                return Json(new { succes = 0, message = "Les dates ne conviennent pas." });
            }
        }

        #endregion


        #region Gestion de la suppresion des temps et des récurrences

        [HttpPost]
        [RulesManager("access")]
        public JsonResult Supprimer(int id)
        {
            temps tps = cnx.temps.Where(t => t.id_temps == id).Single();
            if (tps.frais != null) cnx.DeleteObject(tps.frais);
            cnx.DeleteObject(tps);
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }

        [HttpPost]
        [RulesManager("access")]
        public JsonResult SupprimerRecurrence(int id)
        {
            var temps = cnx.temps.Where(t => t.id_recurrence == id).ToList();
            foreach (var tps in temps)
            {
                cnx.DeleteObject(tps);
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
            temps temps = cnx.temps.Where(t => t.id_temps == id).Single();
            var tps = cnx.temps.Where(t => t.date_debut >= temps.date_debut && t.id_recurrence == temps.id_recurrence).ToList();
            foreach (var t in tps)
            {
                cnx.DeleteObject(t);
            }
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }

        #endregion


        #region Gestion de la duplication d'un évenement

        [HttpGet]
        [RulesManager("access")]
        public PartialViewResult Dupliquer(int id)
        {
            temps tps = cnx.temps.Where(t => t.id_temps == id).Single();
            DupliquerView dpv = new DupliquerView();
            dpv.dateFin = tps.date_debut.AddDays(1);
            dpv.id_temps = id;
            return PartialView("_formDupliquer", dpv);
        }

        [HttpPost]
        [RulesManager("access")]
        public JsonResult Dupliquer(DupliquerView duplicate)
        {
            if (ModelState.IsValid)
            {
                temps tps = cnx.temps.Where(t => t.id_temps == duplicate.id_temps).Single();
                recurrence rec = new recurrence();
                tps.recurrence = rec;
                DateTime debut = tps.date_debut;
                DateTime fin = tps.date_fin;
                bool absence = tps.type_temps.absence;
                int diffDays = duplicate.frequenceInt;
                while (debut.AddDays(diffDays) < duplicate.dateFin.AddDays(1))
                {
                    temps tpsCopie = tps.ClonerPourDupliquer();
                    debut = debut.AddDays(diffDays);
                    fin = fin.AddDays(diffDays);
                    tpsCopie.date_debut = debut;
                    tpsCopie.date_fin = fin;
                    //verification si on le garde ou pas
                    // si c'est un samedi on garde seulement si c'est une astreinte (type_temps=1) ou un congé annuel (type_temps=6)
                    if ((debut.DayOfWeek == DayOfWeek.Saturday) && (tpsCopie.id_type_temps != 1)) continue;
                    // si c'est un dimanche on garde seulement si c'est une astreinte (type_temps=1)
                    if ((debut.DayOfWeek == DayOfWeek.Sunday) && (tpsCopie.id_type_temps != 1)) continue;
                    if (this.IsThereAchevauchement(tpsCopie, absence) == false)
                    {
                        tpsCopie.recurrence = rec;
                        cnx.AddTotemps(tpsCopie);
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

        #endregion


        #region Gestion des favoris

        [RulesManager("access")]
        [HttpPost]
        public PartialViewResult ListeFavori(int id)
        {
            ViewBag.favoris = (from fav in cnx.favori
                               where (fav.id_employe == id)
                               orderby (fav.id_type_temps)
                               select new FavoriView { id_favori = fav.id_favori, nomProjet = fav.projet.nom + " - " + fav.projet.client.nom, nomTache = fav.tache.nom_tache, couleur = fav.type_temps.couleur, id_projetFav = fav.id_projet, id_tacheFav = fav.id_tache, id_type_tempsFav = fav.id_type_temps }).ToList();
            return PartialView("_listeFavori");

        }

        [RulesManager("access")]
        public PartialViewResult PartieFavori()
        {
            var typesTravail = from travail in cnx.type_temps
                               where (travail.absence == false)
                               select new { nomTravail = travail.nom, id_type_travail = travail.id_type_temps };
            ViewBag.id_type_tempsFav = new SelectList(typesTravail, "id_type_travail", "nomTravail");

            var projets = from p in cnx.projet
                          where ( p.z_actif == true)
                          select new { nomProjet = p.reference + " " + p.nom + " / " + p.client.nom, id_projet = p.id_projet };
            ViewBag.id_projetFav = new SelectList(projets, "id_projet", "nomProjet");

            var taches = from task in cnx.tache
                         select new { id_tache = task.id_tache, nom_tache = task.nom_tache };
            ViewBag.id_tacheFav = new SelectList(taches, "id_tache", "nom_tache");

            return PartialView("_formFavori");
        }

        [RulesManager("access")]
        [HttpPost]
        public JsonResult EnregistrerFavori(FavoriFormView favView)
        {
            if (ModelState.IsValid)
            {
                // test des droits de modif
                if (favView.id_employeFav != this.getCurrentUtilisateur().id_employe)
                    return Json(new { succes = 0, message = "Vous n'êtes pas autorisé à modifier les favoris" });
                //vérification que le favori n'existe pas déjà
                if (cnx.favori.Where(f => f.id_type_temps == favView.id_type_tempsFav && f.id_employe == favView.id_employeFav && f.id_projet == favView.id_projetFav && f.id_tache == favView.id_tacheFav).Any())
                    return Json(new { succes = 0, message = "Le favori existe déjà" });
                favori fav = new favori();
                fav.UpdateFromView(favView);
                cnx.AddTofavori(fav);
                cnx.SaveChanges();
                return Json(new { succes = 1 });
            }
            return Json(new { succes = 0, message = "Le favori n'a pas été rajouté" });
        }

        [RulesManager("access")]
        [HttpPost]
        public JsonResult SupprimerFavori(int id)
        {
            favori fav = cnx.favori.Where(f => f.id_favori == id).Single();
            // test des droits de modif
            if (fav.id_employe != this.getCurrentUtilisateur().id_employe)
                return Json(new { succes = 0, message = "Vous n'êtes pas autorisé à modifier les favoris" });
            cnx.DeleteObject(fav);
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }

        #endregion


        public bool IsThereAchevauchement(temps tps, bool absence)
        {
            //recherche de tous les temps qui appartiennent à l'employé et qui sont à cheval avec l'intervalle
            var tempss = cnx.temps.Where(t => t.date_fin > tps.date_debut && t.date_debut < tps.date_fin && t.id_employe == tps.id_employe);
            
            // exclure celui qui correspond à notre temps si cas de modification
            tempss = (tps.id_temps == 0) ? tempss : tempss.Where(t => t.id_temps != tps.id_temps);
            // dans le cas d'une astreinte pas de limitation superposition absence et astreinte
            if (tps.id_type_temps == 1) return tempss.Where(t => t.id_type_temps == 1).Any();
           
            // sinon dans le cas d'une absence, possibilité de recouvrir uen astreinte
            // dans le cas d'un temps de travail pas de possibilité de recouvrement d'une absence
           
            //cette ligne permet la superposition de 2 temps de travail
            return (absence == true) ? tempss.Where(t => t.id_type_temps != 1 && (t.projet.z_actif == true)||(t.projet==null)).Any() : tempss.Where(t => t.type_temps.absence == true).Any();
            
            //cette ligne ne permet pas la superposition de 2 temps de travail hormis astreinte
            //return (absence == true) ? tempss.Where(t => t.id_type_temps != 1 && (t.projet.z_actif == true) || (t.projet == null)).Any() : tempss.Any();
        }


    }
}
