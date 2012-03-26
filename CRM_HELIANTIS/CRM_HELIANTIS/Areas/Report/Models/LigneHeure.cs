using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM_HELIANTIS.Areas.Report.Models
{
    public class LigneHeure
    {
       public DateTime date_debut { get; set; }
        public DateTime date_fin { get; set; }
        public string nom_projet { get; set; }
        public string nom_client { get; set; }
        public string commentaires { get; set; }
        public int type_tps { get; set; }
        public Boolean ? repas_ext { get; set; }
        public string employe_nom { get; set; }
        public string employe_prenom { get; set; }
        public int employe_id { get; set; }
        public string nom_type_tps { get; set; }
 
    }
}