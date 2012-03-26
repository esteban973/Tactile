using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Areas.Planning.Models;
using CRM_HELIANTIS.Areas.Report.Models;

namespace CRM_HELIANTIS.Models
{
    public partial class frais
    {
        public void updateFromView(TravailView travailview){
            this.commentaires = travailview.commentairesFrais;
            this.kilometres = travailview.kilometres;
            this.peage = travailview.peage;
            this.hotel_resto = travailview.hotel_resto;
            this.divers = travailview.divers;
        }

        public void updateFromView(AjoutFrais ajoutFrais)
        {
            this.commentaires = ajoutFrais.commentairesFrais;
            this.kilometres = ajoutFrais.kilometres;
            this.peage = ajoutFrais.peage;
            this.hotel_resto = ajoutFrais.hotel_resto;
            this.divers = ajoutFrais.divers;
        }
    }
}