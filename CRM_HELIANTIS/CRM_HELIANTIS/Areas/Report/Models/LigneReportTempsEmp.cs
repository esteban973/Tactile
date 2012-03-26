using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Report.Models
{
    public class LigneReportTempsEmp
    {
        public DateTime jour { get; set; }
        public string description { get; set; }
        public decimal nbHeures { get; set; }
        public decimal congepaye { get; set; }
        public decimal rtt { get; set; }
        public decimal tempspartiel { get; set; }
        public decimal maladie { get; set; }
        public decimal autreabsence { get; set; }
        public decimal recup { get; set; }
        public int repas_ext { get; set; }
        public int id_employe { get; set; }
        public string nom_employe { get; set; }
        public string prenom_employe { get; set; }

        public LigneReportTempsEmp(DateTime date)
        {
            this.jour = new DateTime(date.Year, date.Month, date.Day);
            this.nbHeures = 0;
            this.congepaye = 0;
            this.rtt = 0;
            this.tempspartiel = 0;
            this.maladie = 0;
            this.autreabsence = 0;
            this.recup = 0;
            this.repas_ext = 0;
            this.description = "";
        }

        public void UpadeEmploye(employe emp)
        {
            this.nom_employe = emp.nom;
            this.prenom_employe = emp.prenom;
            this.id_employe = emp.id_employe;
        }

        public void UpgradeObjet(LigneHeure ligneHeures)
        {
            switch (ligneHeures.type_tps)
            {
                case 6 :
                    this.congepaye+=0.5m;
                    break;
                case 7:
                    this.rtt += 0.5m;
                    break;
                case 8:
                    this.maladie += 0.5m;
                    break;
                case 9:
                    this.recup += 0.5m;
                    break;
                case 10:
                    this.autreabsence += 0.5m;
                    break;
                case 12:
                    this.autreabsence += 0.5m;
                    break;
                case 13:
                    this.autreabsence += 0.5m;
                    break;
                case 14:
                    this.tempspartiel += 0.5m;
                    break;
                default :
                    this.nbHeures += (Decimal)ligneHeures.date_fin.Subtract(ligneHeures.date_debut).TotalHours;
                    break;
            }
            if (this.description != "") this.description = description + "\n";
                this.description = description + " de " + ligneHeures.date_debut.ToShortTimeString() + " à " + ligneHeures.date_fin.ToShortTimeString() + " " + ligneHeures.nom_type_tps + " " + ligneHeures.nom_projet + " " + ligneHeures.nom_client + " " + ligneHeures.commentaires;
            if (ligneHeures.repas_ext == true) this.repas_ext ++;

        }

    }


}