using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Models;
using System.ComponentModel.DataAnnotations;


namespace CRM_HELIANTIS.Areas.Planning.Models
{
    public class AbsenceView
    {
        public int id_temps { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Veuillez renseigner le début")]
        public DateTime dateAbsence { get; set; }


        [Required(ErrorMessage = "Veuillez renseigner l'heure de début")]
        public string finAbsence { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner l'heure de fin")]
        public string debutAbsence { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner le type d'absence")]
        public int type_absence { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner l'id de l'employé")]
        public int id_employe { get; set; }
        [DataType(DataType.MultilineText)]
        public string commentairesTemps { get; set; }

        public void UpdateFromModel(temps t)
        {
            this.id_temps = t.id_temps;
            this.dateAbsence = t.date_debut;
            this.debutAbsence = t.date_debut.ToShortTimeString();
            this.finAbsence = t.date_fin.ToShortTimeString();
            this.type_absence = t.id_type_temps;
            this.id_employe = t.id_employe;
            this.commentairesTemps = t.commentaires;
        }
    }

    public class TravailView
    {
        public int id_temps { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Veuillez renseigner la date")]
        public DateTime dateTravail { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner l'heure de début")]
        public string finTravail { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner l'heure de fin")]
        public string debutTravail { get; set; }
        

        [Required(ErrorMessage = "Veuillez renseigner l'id du type de travail")]
        public int type_travail { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner l'id de l'employé")]
        public int id_employe { get; set; }

        //[Required(ErrorMessage = "Veuillez renseigner l'id du projet")]
        public int? id_projet { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner l'id de la tâche")]
        public int? id_tache { get; set; }

        [Range(0, 2000, ErrorMessage = "Le chiffre doit être compris entre 0 et 2000")]
        public decimal? kilometres { get; set; }

        [Range(0, 2000, ErrorMessage = "Le chiffre doit être compris entre 0 et 2000")]
        public decimal? peage { get; set; }

        [Range(0, 2000, ErrorMessage = "Le chiffre doit être compris entre 0 et 2000")]
        public decimal? hotel_resto { get; set; }

        [Range(0, 2000, ErrorMessage = "Le chiffre doit être compris entre 0 et 2000")]
        public decimal? divers { get; set; }
        public string commentairesFrais { get; set; }
        [DataType(DataType.MultilineText)]
        public string commentairesTemps { get; set; }

        public Boolean repas_ext { get; set; }

       

        public void UpdateFromModel(temps t)
        {
            this.id_temps = t.id_temps;
            this.dateTravail = t.date_debut;
            this.debutTravail = t.date_debut.ToShortTimeString();
            this.finTravail = t.date_fin.ToShortTimeString();
            this.type_travail = t.id_type_temps;
            this.id_employe = t.id_employe;
            this.id_tache = t.id_tache;
            this.id_projet = t.id_projet;
            this.commentairesTemps = t.commentaires;
            this.repas_ext = (bool)t.repas_ext;
            if (t.frais != null)
            {
                this.commentairesFrais = t.frais.commentaires;
                this.kilometres = t.frais.kilometres;
                this.peage = t.frais.peage;
                this.hotel_resto = t.frais.hotel_resto;
                this.divers = t.frais.divers;
            }
        }

        public bool hasFrais()
        {
            if ((this.commentairesFrais != null) || (this.kilometres != null) || (this.peage != null) || (this.hotel_resto != null)) return true;
            else return false;
        }
    }

    public class FavoriFormView
    {
        public int id_employeFav { get; set; }
        public int id_type_tempsFav { get; set; }
        public int id_tacheFav { get; set; }
        public int id_projetFav { get; set; }
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
        public int id_temps { get; set; }

    }

    public class FavoriView
    {
        public int id_favori { get; set; }
        public string couleur { get; set; }
        public string nomProjet { get; set; }
        public string nomTache { get; set; }
        public int id_type_tempsFav { get; set; }
        public int id_tacheFav { get; set; }
        public int id_projetFav { get; set; }
    }

}