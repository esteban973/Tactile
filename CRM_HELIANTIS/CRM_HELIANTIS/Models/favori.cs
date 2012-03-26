using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Areas.Planning.Models;

namespace CRM_HELIANTIS.Models
{
    public partial class favori
    {
        public void UpdateFromView(FavoriFormView favForm)
        {
            this.id_employe = favForm.id_employeFav;
            this.id_projet = favForm.id_projetFav;
            this.id_tache = favForm.id_tacheFav;
            this.id_type_temps = favForm.id_type_tempsFav;
        }
    }
}