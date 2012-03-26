using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Areas.Materiel.Models;
using CRM_HELIANTIS.Areas.Planning.Models;
using CRM_HELIANTIS.Utils;

namespace CRM_HELIANTIS.Models
{
    public partial class temps
    {
        public object PourJson()
        {
            string titreEvent = (this.projet == null) ? this.type_temps.nom : this.type_temps.nom + " - " + this.projet.nom + " / " + this.projet.client.nom;
            
            return new
            {
                id = this.id_temps,
                title = titreEvent,
                color=this.type_temps.couleur,
                id_projet=this.id_projet,
                start = this.date_debut.ToString("o"),
                end = this.date_fin.ToString("o"),
                commentaires = this.commentaires,
                id_employe = this.id_employe,
                absence=this.type_temps.absence,
                recurrence=this.id_recurrence 
            };
        }

        public void UpdateFromView(AbsenceView absenceview)
        {
            this.id_employe = absenceview.id_employe;
            this.date_debut = absenceview.dateAbsence;
            this.date_fin = absenceview.dateAbsence;
            this.date_debut=this.date_debut.ChangeTimeString(absenceview.debutAbsence);
            this.date_fin=this.date_fin.ChangeTimeString(absenceview.finAbsence);
            this.commentaires = absenceview.commentairesTemps;
            this.id_type_temps = absenceview.type_absence;
            this.id_tache = null;
            this.id_projet = null;
            this.repas_ext = false;
        }

        public void UpdateFromView(TravailView travailview)
        {
            this.id_employe = travailview.id_employe;
            this.date_debut = travailview.dateTravail;
            this.date_fin = travailview.dateTravail;
            this.date_debut=this.date_debut.ChangeTimeString(travailview.debutTravail);
            this.date_fin=this.date_fin.ChangeTimeString(travailview.finTravail);
            this.commentaires = travailview.commentairesTemps;
            this.id_type_temps = travailview.type_travail;
            this.id_tache=travailview.id_tache;
            this.id_projet = travailview.id_projet;
            this.repas_ext = travailview.repas_ext;
        }


        public void CorrigerEnDemiJournee(string debMatin, string finMatin, string debAM, string finAM)
        {
            if (this.date_debut.Hour < 12)
            {
                this.date_debut = this.date_debut.ChangeTimeString(debMatin);
                this.date_fin = this.date_debut.ChangeTimeString(finMatin);
            }
            else {
                this.date_debut = this.date_debut.ChangeTimeString(debAM);
                this.date_fin = this.date_debut.ChangeTimeString(finAM);
            }

        }

        public temps ClonerPourDupliquer()
        {
            temps tps = new temps();
            tps.id_employe =this.id_employe;
            tps.commentaires = this.commentaires;
            tps.id_type_temps = this.id_type_temps;
            tps.id_tache = this.id_tache;
            tps.id_projet = this.id_projet;
            tps.repas_ext = false;
            return tps;
        }

       
    }
}