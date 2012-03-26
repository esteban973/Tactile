using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using CRM_HELIANTIS.Models;
using CRM_HELIANTIS.Areas.Materiel.Models;

namespace CRM_HELIANTIS.Models
{
   public partial class materiel
    {
       public void updateFromModel(MaterielView materielview)
       {
           this.nom = materielview.nom;
       }
      
    }

   
}