using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Models;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Areas.Report.Models;
using CRM_HELIANTIS.Areas.Report;
using CRM_HELIANTIS.Utils;
using System.IO;
using CRM_HELIANTIS.Area.Report.Report;
using CRM_HELIANTIS.Global;
using System.Data;




namespace CRM_HELIANTIS.Areas.Report.Controllers
{
    public class IndexController : BaseController
    {
        
        #region Voir les frais

        /// <summary>
        /// renvoie la vue master des frais
        /// </summary>
        
       [RulesManager("access")]
        public ViewResult CalculFrais()
        {
            DateTime dateNow = DateTime.Now;
            ViewBag.year = dateNow.Year;
            ViewBag.month = dateNow.Month;
            return View("vueFrais");
        }

        /// <summary>
        /// renvoie selon exportation la vue partielle, le PDF ou excel
        /// les frais calculés sur le mois pour l'employé courant
        /// </summary>
       
        [RulesManager("access")]
        public ActionResult CalculFraisPartial(int year, int month, string exportation)
        {
            if (month == 1)
            {
                year = year - 1;
                month = 12;
            }
            DateTime dateDeb = new DateTime(year, month - 1, 26, 5, 0, 0);
            DateTime dateFin = new DateTime(year, month, 25, 22, 0, 0);
            employe idEmp = this.getCurrentUtilisateur();

            List<FraisDetail> fraisListe = Reports.calculFrais(dateDeb, dateFin, idEmp.id_employe);
            if (exportation == "html")
            {
                ViewBag.basefrais = Constants.BASEFRAISKILOMETRIK;
                return PartialView("_CalculFraisPartial", fraisListe);
            }
            if (exportation == "PDF") return this.ReportViewRenderPdf(Server.MapPath("~/Areas/Report/Report/fraisEmploye.rdlc"), "fraisEmploye", fraisListe, "Frais_" + idEmp.nom + "_" + month + "_" + year + ".pdf");
            else return this.ReportViewRenderExcel(Server.MapPath("~/Areas/Report/Report/fraisEmploye.rdlc"), "fraisEmploye", fraisListe, "Frais_" + idEmp.nom + "_" + month + "_" + year + ".xls");
        }

        /// <summary>
        /// modifie la valeur des frais envoyes à partir de la vue des frais
        /// </summary>

        [RulesManager("access")]
        [HttpPost]
        public JsonResult modifierFrais(string value, int id, string frais)
        {
            
            var expense = cnx.frais.Where(f => f.id_frais == id).FirstOrDefault();
            var temps=expense.temps.FirstOrDefault();
            if (temps.id_employe != this.getCurrentUtilisateur().id_employe) return Json(null);
            decimal ? val;
            if (value == "") val = null;
            else try
                {
                    val = Convert.ToDecimal(value.Replace('.', ','));
                }
                catch (Exception)
                {
                    return Json(new { succes = 0, message = "Erreur de format" });
                }
            switch (frais)
            {
                case "parking": expense.peage= val;
                    break;
                case "resto": expense.hotel_resto = val;
                    break;
                case "km": expense.kilometres = val;
                    break;
                case "divers": expense.divers = val;
                    break;
            }
            if (expense.divers == null && expense.peage == null && expense.hotel_resto == null && expense.divers == null)
            {
                temps.id_frais = null;
                cnx.DeleteObject(expense);
                cnx.ObjectStateManager.ChangeObjectState(temps, EntityState.Modified);
                cnx.SaveChanges();
                return Json(new { succes = 2, message = "Efface la ligne" });
            }
            else {
                cnx.ObjectStateManager.ChangeObjectState(expense, EntityState.Modified);
                cnx.SaveChanges();
            }
            return Json(new { succes = 1, message = "Actualise la ligne" });
        }

        /// <summary>
        /// modifie le libelle à partir de la vue des frais
        /// </summary>

        [RulesManager("access")]
        [HttpPost]
        public JsonResult modifierLibelleFrais(string value, int id)
        {

            var expense = cnx.frais.Where(f => f.id_frais == id).FirstOrDefault();
            var temps = expense.temps.FirstOrDefault();
            if (temps.id_employe != this.getCurrentUtilisateur().id_employe) return Json(null);
            expense.commentaires = value;
            cnx.ObjectStateManager.ChangeObjectState(expense, EntityState.Modified);
            cnx.SaveChanges();
            return Json(new { succes = 1, message = "Actualise la ligne" });
        }

        



        #endregion


        #region voir les temps de travail des employes

        /// <summary>
        /// renvoie la vue master des temps (pas de distinction de projet) pour un employé
        /// </summary>
        [RulesManager("access")]
        public ViewResult voirTemps()
        {
            DateTime dateNow = DateTime.Now;
            ViewBag.year = dateNow.Year;
            ViewBag.month = dateNow.Month;
            return View("vueTemps");
        }


        /// <summary>
        /// renvoie la vue master des temps (pas de distinction de projet) pour tous les employés (côté administrateur)
        /// </summary>
        [RulesManager("can_print_time_employe")]
        public ViewResult voirTempsEmployes()
        {
            DateTime dateNow = DateTime.Now;
            ViewBag.year = dateNow.Year;
            ViewBag.month = dateNow.Month;
            var employes = from e in cnx.employe
                           where (e.z_actif == true)
                           select new { nomComplet = e.prenom + " " + e.nom.ToUpper(), id_employe = e.id_employe };
            ViewBag.employeList = new SelectList(employes, "id_employe", "nomComplet", this.getCurrentUtilisateur().id_employe);
            return View("vueTemps");
        }

        /// <summary>
        /// renvoie selon exportation la vue partielle, le PDF ou excel
        /// les temps d'un employé sur un mois et une année donnée
        /// </summary>
        [RulesManager("access")]
        public ActionResult voirTempsPartial(int year, int month, int idEmploye, string exportation)
        {
            if (!this.verifAccesSpecif(idEmploye)) return new ViewResult { ViewName = "ErrorAuthorization" };
            List<employe> emps = new List<employe>();
            var employe = cnx.employe.Where(e => e.id_employe == idEmploye).Single();
            emps.Add(employe);
            List<LigneReportTempsEmp> listeJourMois = Reports.calculTemps(year, month, emps);
            switch (exportation)
            {
                case "html": return View("_vueTemps", listeJourMois);
                case "PDF": return this.ReportViewRenderPdf(Server.MapPath("~/Areas/Report/Report/tempsEmploye.rdlc"), "TempsEmployeData", listeJourMois, "Temps_" + employe.nom + "_" + month + "_" + year + ".pdf");
                default: return this.ReportViewRenderExcel(Server.MapPath("~/Areas/Report/Report/tempsEmploye.rdlc"), "TempsEmployeData", listeJourMois, "Temps_" + employe.nom + "_" + month + "_" + year + ".xls");
            }
            
        }

        /// <summary>
        /// renvoie selon exportation le PDF ou excel
        /// de tous les temps de tous les employés pour un mois et une année donnée
        /// </summary>
        [RulesManager("can_print_time_employe")]
        public ActionResult imprimerTempsEmployes(int year, int month, string exportation)
        {
            List <employe> emps = cnx.employe.Where(e => e.z_actif==true).ToList();
            List<LigneReportTempsEmp> listeJourMois = Reports.calculTemps(year, month, emps);
            if (exportation=="PDF") return this.ReportViewRenderPdf(Server.MapPath("~/Areas/Report/Report/tempsEmploye.rdlc"), "TempsEmployeData", listeJourMois, "Temps_"  + month + "_" + year + ".pdf");
            return this.ReportViewRenderExcel(Server.MapPath("~/Areas/Report/Report/tempsEmploye.rdlc"), "TempsEmployeData", listeJourMois, "Temps_" + month + "_" + year + ".xls");
        }



               
        #endregion



         #region voir les temps de travil d'un employe sur un projet ou voir tous les temps de tous les employes sur un projet


        /// <summary>
        /// renvoie la vue master des projets pour un employé
        /// </summary>
         [RulesManager("access")]
         public ViewResult TempsProjet()
         {
             int idEmp=this.getCurrentUtilisateur().id_employe;                    
             var projets = (from tps in cnx.temps
                            where (tps.id_employe == idEmp)
                            select new {id_projet=tps.id_projet, nom_projet=tps.projet.nom+" - "+tps.projet.client.nom}).Distinct().ToList();
             
             ViewBag.listeProjet=new SelectList(projets, "id_projet", "nom_projet");                           
             return View("vueProjet");
         }

         /// <summary>
         /// renvoie la vue master des projets pour les administrateurs
         /// </summary>
        [RulesManager("can_print_time_projet")]
        public ViewResult TempsProjetAdministrateur()
        {
            var projets = (from proj in cnx.projet
                           where (proj.z_actif == true)
                           select new { id_projet = proj.id_projet, nom_projet = proj.nom + " - " + proj.client.nom }).ToList();
            ViewBag.listeProjet = new SelectList(projets, "id_projet", "nom_projet");
            var employes = from e in cnx.employe
                           where (e.z_actif == true)
                           select new { nomComplet = e.prenom + " " + e.nom.ToUpper(), id_employe = e.id_employe };
            ViewBag.employeList = new SelectList(employes, "id_employe", "nomComplet");
            return View("vueProjetAdministrateur");
        }


        /// <summary>
        /// renvoie selon exportation la vue partielle, le PDF ou excel
        /// de tous les temps d'un employé, sur un projet (optionnel : pour un mois et une année donnée)
        /// </summary>
        [RulesManager("access")]
        public ActionResult TempsProjetPartial(int idEmploye, int idProjet, string exportation, int? year, int? month)
        {
            List<TempsProjetListe> listeTempsProjet;
            if (year.HasValue && month.HasValue) listeTempsProjet = Reports.calculTempsProjetEmploye(idEmploye, idProjet, year, month);
            else listeTempsProjet = Reports.calculTempsProjetEmploye(idEmploye, idProjet);

            if (exportation == "html") return PartialView("_vueProjet", listeTempsProjet);
            var nomprojet = cnx.projet.Where(p => p.id_projet == idProjet).FirstOrDefault().nom;
            if (exportation == "PDF") return this.ReportViewRenderPdf(Server.MapPath("~/Areas/Report/Report/vueTempsProjet.rdlc"), "tempsProjetData", listeTempsProjet, "Temps_" + nomprojet + ".pdf");
            else return this.ReportViewRenderExcel(Server.MapPath("~/Areas/Report/Report/vueTempsProjet.rdlc"), "tempsProjetData", listeTempsProjet, "Temps_" + nomprojet + ".xls");
        }


        /// <summary>
        /// renvoie selon exportation la vue partielle, le PDF ou excel
        /// de tous les temps de tous les employés sur un projet (optionnel : pour un mois et une année donnée)
        /// </summary>
       [RulesManager("can_print_time_projet")]
        public ActionResult TempsProjetTous(int idProjet, string exportation, int? year, int? month)
        {
            List<TempsProjetListe> listeTempsProjet;
            if (year.HasValue && month.HasValue) listeTempsProjet = Reports.calculTempsProjetEmploye(idProjet, (int)year, (int)month);
            else listeTempsProjet = Reports.calculTempsProjetEmploye(idProjet);
           
            if (exportation == "html") return PartialView("_vueProjetTous", listeTempsProjet);
            var nomprojet = cnx.projet.Where(p => p.id_projet == idProjet).FirstOrDefault().nom;
            if (exportation == "PDF") return this.ReportViewRenderPdf(Server.MapPath("~/Areas/Report/Report/vueTempsProjet.rdlc"), "tempsProjetData", listeTempsProjet, "Temps_Tous_" + nomprojet + ".pdf");
            return this.ReportViewRenderExcel(Server.MapPath("~/Areas/Report/Report/vueTempsProjet.rdlc"), "tempsProjetData", listeTempsProjet, "Temps_Tous_" + nomprojet + ".xls");
        }

       /// <summary>
       /// renvoie selon exportation la vue partielle, le PDF ou excel
       /// de tous les temps globaux de tous les employés sur un projet, et de tous les temps par tache 
       /// (optionnel : pour un mois et une année donnée)
       /// </summary>
       [RulesManager("can_print_time_projet")]
       public ActionResult TempsProjetGlobal(int idProjet, string exportation, int? year, int? month)
       {
           List<TempsProjetEmploye> listeTempsEmployeProjet;
           List<TempsProjetTache> listeTache;
           if (year.HasValue && month.HasValue)
           {
               listeTempsEmployeProjet = Reports.calculTempsProjetEmployeGlobal(idProjet, (int)year, (int)month);
               listeTache = Reports.calculTempsProjetTache(idProjet, (int)year, (int)month);
           }
           else
           {
               listeTempsEmployeProjet = Reports.calculTempsProjetEmployeGlobal(idProjet);
               listeTache = Reports.calculTempsProjetTache(idProjet);
           }
           if (exportation == "html")
           {
               ViewBag.listeTache = listeTache;
               return PartialView("_vueProjetEmployeGlobal", listeTempsEmployeProjet);
           }
           var nomprojet = Reports.nomProjet(idProjet);
           Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
           dictionary.Add("tempsEmployeProjetGlobalData", listeTempsEmployeProjet);
           dictionary.Add("tempsEmployeTacheData", listeTache);
           dictionary.Add("nomProjetData", nomprojet);
           if (exportation == "PDF") return this.ReportViewRenderPdf(Server.MapPath("~/Areas/Report/Report/tempsGlobalEmploye.rdlc"), dictionary, "Temps_Tous_" + nomprojet + ".pdf");
           else return this.ReportViewRenderExcel(Server.MapPath("~/Areas/Report/Report/tempsGlobalEmploye.rdlc"), dictionary, "Temps_Tous_" + nomprojet.FirstOrDefault().nom + ".xls");
       }

        #endregion

        /// <summary>
        /// retourne si un utilisateur peut accéder à cette fonction
        /// </summary>

         public Boolean verifAccesSpecif(int idEmp)
         {
             if ((this.getCurrentUtilisateur().id_employe != idEmp) && (!this.isAuthorized("can_print_time_employe"))) return false;
             else return true;
         }
    }
}

