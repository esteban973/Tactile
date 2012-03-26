using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using CRM_HELIANTIS.Models;
using CRM_HELIANTIS.Areas.Utilisateur.Models;

namespace CRM_HELIANTIS.Models
{
   
    public partial class employe
    {
       public string nomComplet()
       {
           return this.prenom + " " + this.nom.ToUpper();
       }

       public Boolean isUnique(){
              HeliantisEntities cnx=new HeliantisEntities();
           
              var nb = cnx.utilisateur.Where(u => u.identifiant ==this.utilisateur.identifiant && u.id_utilisateur!=this.utilisateur.id_utilisateur && u.employe.FirstOrDefault().z_actif==true).Count();
              if (nb == 0) return true;
              else return false;
       }

       public void updateFromView(UtilisateurView userview)
       {
           this.nom = userview.nom;
           this.prenom = userview.prenom;
           this.email = userview.email;
           this.id_groupe = userview.id_groupe;
           this.id_tache = userview.id_tache;
           this.id_role = userview.id_role;
           this.horaires_matin = userview.horaires_matin;
           this.horaires_matin_fin = userview.horaires_matin_fin;
           this.horaires_apresmidi = userview.horaires_apresmidi;
           this.horaires_apresmidi_fin = userview.horaires_apresmidi_fin;
           this.z_actif = true;
       }

       

    }

}