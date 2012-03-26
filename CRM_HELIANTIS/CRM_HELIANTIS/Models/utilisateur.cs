using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using CRM_HELIANTIS.Areas.Utilisateur.Models;


namespace CRM_HELIANTIS.Models
{
    
    public partial class utilisateur
    {
        public void UpdateFromView(UtilisateurView userview)
        {
            this.identifiant = userview.identifiant;
        }

    }

     
}