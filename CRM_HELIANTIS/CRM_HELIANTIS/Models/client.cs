using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using CRM_HELIANTIS.Models;
using CRM_HELIANTIS.Areas.Projet.Models;

namespace CRM_HELIANTIS.Models
{
   public partial class client
    {
       public void updateFromModel(ClientView clientview)
       {
           this.nom = clientview.nom;
           this.reference = clientview.reference;
           this.z_actif = true;
       }
      
    }

   
}