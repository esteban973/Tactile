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
    public partial class projet
    {
       public void updateFromModel(ProjetView projetview)
       {
           this.nom = projetview.nom;
           this.reference = projetview.reference;
           this.id_client = projetview.id_client;
           this.z_actif = true;
       }
      
    }

   
}