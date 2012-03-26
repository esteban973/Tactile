using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Models;
using System.ComponentModel.DataAnnotations;

namespace CRM_HELIANTIS.Areas.Materiel.Models
{
    public class ReservationView
    {
        public string commentaires  { get; set; }
        public int id_resa { get; set; }
        public DateTime debut { get; set; }
        public DateTime fin { get; set; }
        public int id_matos { get; set; }
        public int id_employe { get; set; }

        public void updateFromModel(reservation resa) {
            this.id_employe = resa.id_employe;
            this.commentaires = resa.commentaires;
            this.fin = resa.date_fin;
            this.debut = resa.date_debut;
            this.id_matos = resa.id_materiel;
            this.id_resa = resa.id_reservation;
        }
    }

    public class DupliquerView
    {
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Veuillez renseigner la fin")]
        public DateTime dateFin { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner la fréquence")]
        public int frequenceInt { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner l'id de l'évenement")]
        public int id_resa { get; set; }

    }


}