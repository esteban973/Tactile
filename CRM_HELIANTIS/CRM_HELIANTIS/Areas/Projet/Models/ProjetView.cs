using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Projet.Models
{
    public class ProjetView
    {
        public string reference { get; set; }
        public int id { get; set; }
        public string nom { get; set; }
        public int id_client { get; set; }
       

        public void updateFromModel(projet projet) {
            this.id = projet.id_projet;
            this.nom = projet.nom;
            this.reference = projet.reference;
            this.id_client = projet.id_client;
            
        }

    }

    
}
