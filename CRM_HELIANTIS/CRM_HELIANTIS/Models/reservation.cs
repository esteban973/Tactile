using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Areas.Materiel.Models;

namespace CRM_HELIANTIS.Models
{
    public partial class reservation
    {
        public object PourJson()
        {
            return new
            {
                id = this.id_reservation,
                title = this.employe.nomComplet(),
                start = this.date_debut.ToString("o"),
                end = this.date_fin.ToString("o"),
                commentaires = this.commentaires,
                id_materiel = this.id_materiel,
                id_employe = this.id_employe,
                recurrence=this.id_recurrence
            };
        }

        public void UpdateFromModelView(ReservationView resaview)
        {
            this.id_employe = resaview.id_employe;
            this.commentaires = (String.IsNullOrEmpty(resaview.commentaires)) ? "" : resaview.commentaires;
            this.date_fin = resaview.fin;
            this.date_debut = resaview.debut;
            this.id_materiel = resaview.id_matos;
        }

        public reservation ClonerPourDupliquer()
        {
            reservation resa = new reservation();
            resa.id_employe = this.id_employe;
            resa.id_materiel = this.id_materiel;
            resa.commentaires = this.commentaires;
            return resa;
        }
    }
}