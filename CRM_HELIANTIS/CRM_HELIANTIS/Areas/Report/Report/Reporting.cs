using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Objects;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections;
using System.Text;
using System.Linq.Dynamic;
using System.Dynamic;
using CRM_HELIANTIS.Areas.Report.Models;
using CRM_HELIANTIS.Models;
using System.Web.Mvc;
using System.Linq;
using System.Globalization;


namespace CRM_HELIANTIS.Area.Report.Report
{
    public class Reports
    {
        /// <summary>
        /// retourne pour une liste d'employé, un mois et une année donnée, la liste de tous les temps de travail 
        /// organisé en objets LigneReportTempsEmp
        /// </summary>
        
        public static List<LigneReportTempsEmp> calculTemps(int year, int month, List<employe> emps)
        {

            HeliantisEntities cnx = new HeliantisEntities();
            List<LigneReportTempsEmp> retourListe = new List<LigneReportTempsEmp>();
            List<LigneHeure> listeHeures = (from tps in cnx.temps
                                            where (tps.date_debut.Month == month && tps.date_debut.Year == year && tps.id_type_temps != 1 && tps.employe.z_actif == true)
                                            select new LigneHeure
                                            {
                                                date_debut = tps.date_debut,
                                                date_fin = tps.date_fin,
                                                nom_projet = tps.projet.nom,
                                                nom_client = tps.projet.client.nom,
                                                commentaires = tps.commentaires,
                                                type_tps = tps.id_type_temps,
                                                nom_type_tps = tps.type_temps.nom,
                                                repas_ext = tps.repas_ext,
                                                employe_nom = tps.employe.nom,
                                                employe_prenom = tps.employe.prenom,
                                                employe_id = tps.employe.id_employe
                                            }).ToList();

            foreach (var emp in emps)
            {
                DateTime date = new DateTime(year, month, 1);
                while (date.Month == month)
                {
                    if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        date = date.AddDays(1);
                        continue;
                    }
                    LigneReportTempsEmp ligne = new LigneReportTempsEmp(date);
                    retourListe.Add(ligne);
                    ligne.UpadeEmploye(emp);
                    var listeHeure = listeHeures.Where(l => l.date_debut.ToShortDateString() == date.ToShortDateString() && l.employe_id == emp.id_employe).ToList();
                    foreach (var lstHeure in listeHeure)
                    {
                        ligne.UpgradeObjet(lstHeure);
                    }
                    date = date.AddDays(1);
                }
            }
            return retourListe;
        }

        /// <summary>
        /// retourne la liste des frais d'un employé entre 2 dates données
        /// </summary>
       

        public static List<FraisDetail> calculFrais(DateTime dateDeb, DateTime dateFin, int idEmp)
        {
            HeliantisEntities cnx = new HeliantisEntities();
            List<FraisDetail> fraisListe = (from expense in cnx.frais
                                            where (expense.temps.FirstOrDefault().id_employe == idEmp && expense.temps.FirstOrDefault().date_debut >= dateDeb && expense.temps.FirstOrDefault().date_debut <= dateFin)
                                            orderby (expense.temps.FirstOrDefault().date_debut)
                                            select new FraisDetail
                                            {
                                                datefrais = expense.temps.FirstOrDefault().date_debut,
                                                divers = expense.divers,
                                                libelle = expense.commentaires,
                                                km = expense.kilometres,
                                                parking = expense.peage,
                                                projet = expense.temps.FirstOrDefault().projet.nom + " " + expense.temps.FirstOrDefault().projet.client.nom,
                                                resto = expense.hotel_resto,
                                                id_employe = expense.temps.FirstOrDefault().id_employe,
                                                nom_employe = expense.temps.FirstOrDefault().employe.nom,
                                                prenom_employe = expense.temps.FirstOrDefault().employe.prenom,
                                                id_frais=expense.id_frais,
                                                type_temps=expense.temps.FirstOrDefault().type_temps.nom
                                            }).ToList();
            cnx.Dispose();
            return fraisListe;
        }


        /// <summary>
        /// retourne tous les temps passés par tousles employés sur un projet
        /// </summary>
        public static List<TempsProjetListe> calculTempsProjetEmploye(int idProjet)
        {
            return calculTempsProjetEmploye(null, idProjet, null, null);
        }

        /// <summary>
        /// retourne tous les temps passés par un employé sur un projet
        /// </summary>
        public static List<TempsProjetListe> calculTempsProjetEmploye(int idEmp, int idProjet)
        {
            return calculTempsProjetEmploye(idEmp, idProjet, null, null);
        }

        /// <summary>
        /// retourne tous les temps passés par tousles employés sur un projet, sur un mois et une année donnée
        /// </summary>
        public static List<TempsProjetListe> calculTempsProjetEmploye(int idProjet, int year, int month)
        {
            return calculTempsProjetEmploye(null, idProjet, year, month);
        }

        /// <summary>
        /// retourne tous les temps passés par un employé sur un projet, sur un mois et une année donnée
        /// </summary>
              public static List<TempsProjetListe> calculTempsProjetEmploye(int? idEmp, int idProjet, int? year, int? month)
        {
            HeliantisEntities cnx=new HeliantisEntities();
                        var tempsprojetListe = from t in cnx.temps
                                   where (t.id_projet == idProjet)
                                   orderby (t.date_debut)
                                   select new TempsProjetListe
                                   {
                                       nom_projet = t.projet.nom,
                                       client = t.projet.client.nom,
                                       datedeb = t.date_debut,
                                       datefin = t.date_fin,
                                       tache = t.tache.nom_tache,
                                       type = t.type_temps.nom,
                                       commentaires = t.commentaires,
                                       id_employe = t.id_employe,
                                       nom_employe = t.employe.nom,
                                       prenom_employe = t.employe.prenom
                                   };
            if (idEmp.HasValue) tempsprojetListe = tempsprojetListe.Where(t => t.id_employe == idEmp);
            if (year.HasValue) tempsprojetListe = tempsprojetListe.Where(t => t.datedeb.Month == month && t.datedeb.Year == year);
            return tempsprojetListe.ToList();
        }


        /// <summary>
        /// retourne le temps global passé par un employé sur un projet
        /// </summary>

        public static List<TempsProjetEmploye> calculTempsProjetEmployeGlobal(int idProjet)
        {
            return calculTempsProjetEmployeGlobal(idProjet, null, null);
        }

        /// <summary>
        /// retourne le temps global passé par un employé sur un projet, pour un mois et une année donnée
        /// </summary>
        public static List<TempsProjetEmploye> calculTempsProjetEmployeGlobal(int idProjet, int? year, int? month)
        {
            HeliantisEntities cnx = new HeliantisEntities();
            var tempsprojet = cnx.temps.Where(t => t.id_projet == idProjet).
                             Select(t => new CalcultempsTmp { nom = t.employe.nom + " " + t.employe.prenom, id_employe = t.id_employe, dateDeb = t.date_debut, dateFin = t.date_fin, groupe = t.employe.groupe.nom });
            if (year.HasValue) tempsprojet = tempsprojet.Where(t => t.dateDeb.Month == month && t.dateFin.Year == year);
            var totalprojet = tempsprojet.ToList().GroupBy(t => t.id_employe).Select(tg => new TempsProjetEmploye { 
                nomComplet = tg.FirstOrDefault().nom, 
                nomService=tg.FirstOrDefault().groupe, 
                temps = tg.Sum(t => t.dateFin.Subtract(t.dateDeb).TotalHours) }).ToList();
            return totalprojet;
        }

        /// <summary>
        /// retourne le temps global passé pour un projet donné par tâche
        /// </summary>

        public static List<TempsProjetTache> calculTempsProjetTache(int idProjet)
        {
            return calculTempsProjetTache(idProjet, null, null);
        }
       
        /// <summary>
        /// retourne le temps global passé pour un projet donné par tâche
        /// </summary>
      
        public static List<TempsProjetTache> calculTempsProjetTache(int idProjet , int? year, int? month)
        {
            HeliantisEntities cnx = new HeliantisEntities();
            var tempsprojet = cnx.temps.Where(t => t.id_projet == idProjet).Select(t => new CalcultempsTmp { nom = t.tache.nom_tache, id_tache = t.id_tache, dateDeb = t.date_debut, dateFin = t.date_fin, projet = t.projet.reference + " " + t.projet.nom + " / " + t.projet.client.reference + " " + t.projet.client.nom });
            if (year.HasValue) tempsprojet = tempsprojet.Where(t => t.dateDeb.Month == month && t.dateFin.Year == year);
            var totalprojet = tempsprojet.ToList().GroupBy(t => t.id_tache).Select(tg => new TempsProjetTache
            {
                nomTache = tg.FirstOrDefault().nom,
                nomProjet = tg.FirstOrDefault().projet,
                temps = tg.Sum(t => t.dateFin.Subtract(t.dateDeb).TotalHours)
            }).ToList();
            return totalprojet;
        }


        /// <summary>
        /// retourne le nom d'un projet (utilisé par l'outil de reporting)
        /// </summary>
       
        public static List<NomProj> nomProjet(int idProjet)
        {
            HeliantisEntities cnx = new HeliantisEntities();
            var nomprojet = cnx.projet.Where(t => t.id_projet == idProjet).Select(t => new NomProj{ nom = t.reference + " " + t.nom + " / " + t.client.reference + " " + t.client.nom }).ToList();
            return nomprojet;
        }
        
        /// <summary>
        /// classe temporaire utilisée dans les requetes linq complexes
        /// </summary>
        public class CalcultempsTmp {

            public string nom { get; set; }
            public DateTime dateDeb { get; set; }
            public DateTime dateFin { get; set; }
            public int id_employe { get; set; }
            public string groupe { get; set; }
            public string projet { get; set; }
            public int? id_tache { get; set; }

        }

    }

}