using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Utilisateur.Models
{
    public class UtilisateurView
    {
        public int ? id { get; set; }
        [Required(ErrorMessage = "Vous devez renseigner le nom")]
        [MinLength(2, ErrorMessage = "Le nom doit être au moins de 2 caractères")]
        public string nom { get; set; }
        [Required(ErrorMessage = "Vous devez renseigner le service")]
        public int ? id_groupe { get; set; }
        [Required(ErrorMessage = "Vous devez renseigner le rôle")]
        public int ? id_role{ get; set; }
        [Required(ErrorMessage = "Vous devez renseigner la tâche")]
        public int? id_tache { get; set; }
        [RegularExpression(@"[0-1][0-9]:[0-5][0-9]\s*", ErrorMessage = "Les heures doivent être indiquées au format hh:mm")]
        public string horaires_matin { get; set; }
        [RegularExpression(@"[0-1][0-9]:[0-5][0-9]\s*", ErrorMessage = "Les heures doivent être indiquées au format hh:mm")]
        public string horaires_matin_fin { get; set; }
        [RegularExpression(@"[0-1][0-9]:[0-5][0-9]\s*", ErrorMessage = "Les heures doivent être indiquées au format hh:mm")]
        public string horaires_apresmidi { get; set; }
        [RegularExpression(@"[0-1][0-9]:[0-5][0-9]\s*", ErrorMessage = "Les heures doivent être indiquées au format hh:mm")]
        public string horaires_apresmidi_fin { get; set; }

        [Required(ErrorMessage = "Vous devez renseigner l'identifiant")]
        public string identifiant { get; set; }
        public string prenom { get; set; }
        public string email { get; set; }


        public void updateFromModel(employe emp)
        {
            this.id=emp.id_employe;
            this.nom=emp.nom;
            this.prenom = emp.prenom;
            this.email = emp.email;
            this.id_groupe = emp.id_groupe;
            this.id_role = emp.id_role;
            this.id_tache = emp.id_tache;
            this.horaires_matin = emp.horaires_matin;
            this.horaires_matin_fin = emp.horaires_matin_fin;
            this.horaires_apresmidi_fin = emp.horaires_apresmidi_fin;
            this.horaires_apresmidi = emp.horaires_apresmidi;
            this.identifiant = emp.utilisateur.identifiant;
        }
    }

}