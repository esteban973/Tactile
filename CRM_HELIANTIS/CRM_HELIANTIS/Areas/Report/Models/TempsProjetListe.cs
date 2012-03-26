using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM_HELIANTIS.Areas.Report.Models
{
    public class TempsProjetListe
    {   
        public string nom_projet { get; set; }
        public  string  client {get;set;}
        public DateTime datedeb { get; set; }
        public DateTime datefin { get; set; }
        public string tache { get; set; }
        public string type { get; set; }
        public string commentaires { get; set; }
        public string nom_employe { get; set; }
        public string prenom_employe { get; set; }
        public int id_employe { get; set; }
    }
}