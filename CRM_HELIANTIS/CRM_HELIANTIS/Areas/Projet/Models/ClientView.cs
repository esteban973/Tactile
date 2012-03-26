using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_HELIANTIS.Models;
using System.ComponentModel.DataAnnotations;

namespace CRM_HELIANTIS.Areas.Projet.Models
{
    public class ClientView
    {
        [Required(ErrorMessage = "Veuillez renseigner la référence")]
        public string reference { get; set; }
        public int id { get; set; }
        [Required(ErrorMessage="Veuillez renseigner le nom")]
        public string nom { get; set; }
        


        public void updateFromModel(client client) {
            this.nom = client.nom;
            this.reference = client.reference;
            this.id = client.id_client;
        }

    }

    
}
