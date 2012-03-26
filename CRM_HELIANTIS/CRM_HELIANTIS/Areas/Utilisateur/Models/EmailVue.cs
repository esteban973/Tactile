using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Utilisateur.Models
{
    public class EmailInfoConnexion
    {      
        public string nom_employe { get; set; }
        public string prenom_employe { get; set; }
        public string identifiant { get; set; }
        public string password { get; set; }
        public string horairesMat { get; set; }
        public string horairesApres { get; set; }
        public string email { get; set; }
        public string tache { get; set; }
        public string service { get; set; }

        public EmailInfoConnexion(employe emp)
        {
            this.nom_employe = emp.nom;
            this.prenom_employe = emp.prenom;
            this.identifiant = emp.utilisateur.identifiant;
            this.password = emp.utilisateur.mot_passe;
            this.horairesMat = emp.horaires_matin + "-" + emp.horaires_matin_fin;
            this.horairesApres = emp.horaires_apresmidi + "-" + emp.horaires_apresmidi_fin;
            this.email = emp.email;
            this.tache = emp.tache.nom_tache;
            this.service = emp.groupe.nom;
        }
    
    }


}