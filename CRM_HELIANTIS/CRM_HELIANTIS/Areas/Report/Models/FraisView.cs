using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Global;

namespace CRM_HELIANTIS.Areas.Report.Models
{
     public class FraisDetail
        {
            public DateTime datefrais { get; set; }
            public string libelle { get; set; }
            public string projet { get; set; }
            public decimal? parking { get; set; }
            public decimal? km { get; set; }
            public decimal? resto {get;set;}
            public decimal? divers { get; set; }
            public int id_employe { get; set; }
            public string nom_employe { get; set; }
            public string prenom_employe { get; set; }
            public string type_temps { get; set; }
            public int id_frais { get; set; }

        }

    
    
}