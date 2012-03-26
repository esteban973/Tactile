using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Models;
using CRM_HELIANTIS.Classes;
using System.Data;
using System.IO;
using CRM_HELIANTIS.Areas.Utilisateur.Models;
using CRM_HELIANTIS.Utils;

namespace CRM_HELIANTIS.Areas.Utilisateur.Controllers
{
    public class IndexController : BaseController
    {


        /// <summary>
        /// Renvoie la vue générale avec le tableau des utilisateurs
        /// </summary>
        /// <returns></returns>

        [RulesManager("can_manage_employes")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Renvoie un tableau en Json , pour alimenter le tableau des utilisateurs
        /// </summary>
     

        [RulesManager("can_manage_employes")]
        public JsonResult listeUtilisateur()
        {
            var employes = cnx.employe.Where(e=>e.z_actif==true).ToList();
            List<object> retour = new List<object>();
            foreach (var employe in employes)
            {
                retour.Add(new
                {
                    id = employe.id_employe,
                    nom = employe.nom.ToUpper(),
                    prenom = employe.prenom,
                    identifiant =employe.utilisateur.identifiant,
                    role = employe.role.role_description,
                    horaires = employe.horaires_matin+" - " + employe.horaires_matin_fin+ " / "+ employe.horaires_apresmidi+" - "+ employe.horaires_apresmidi_fin,
                    email = employe.email,
                    groupe = employe.groupe.nom,
                    bouton = "<span class='edit' data-id=" + employe.id_employe + ">Editer</span><span class='suppr' data-identite='" + employe.nomComplet() + "' data-id=" + employe.id_employe + ">Supprimer</span>"
                });
            }
            return Json(retour, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Renvoie le formulaire de création d'un utilisateur
        /// </summary>
        [RulesManager("can_manage_employes")]
        public PartialViewResult Creer()
        {
            ViewBag.type = "creer";
            ViewBag.id_groupe = new SelectList(cnx.groupe, "id_groupe", "nom");
            ViewBag.id_role = new SelectList(cnx.role, "id_role", "role_description");
            ViewBag.id_tache = new SelectList(cnx.tache, "id_tache", "nom_tache");
            return PartialView("_formUtilisateur");
        }

        /// <summary>
        /// Renvoie le formulaire d'édition d'un utilisateur
        /// </summary>
        [RulesManager("can_manage_employes")]
        public PartialViewResult Editer(int id)
        {
            ViewBag.type = "editer";
            employe employe = cnx.employe.Single(e => e.id_employe == id);
            UtilisateurView userview = new UtilisateurView();
            userview.updateFromModel(employe);
            ViewBag.id_groupe = new SelectList(cnx.groupe, "id_groupe", "nom", employe.id_groupe);
            ViewBag.id_role = new SelectList(cnx.role, "id_role", "role_description", employe.id_role);
            ViewBag.id_tache = new SelectList(cnx.tache, "id_tache", "nom_tache", employe.id_tache);
            return PartialView("_formUtilisateur", userview);
        }


        /// <summary>
        /// Crée un objet employé et utilisateur lié à partir d'un Ojet UtilisateurView
        /// Si création attribue un mot de passe aléatoire et envoie un email avec toutes les infos de connexion
        /// Retour en Json
        /// </summary>
        [RulesManager("can_manage_employes")]
        [HttpPost]
        public ActionResult Editer(UtilisateurView userview)
        {
            if (ModelState.IsValid)
            {
                employe emp;
                if (userview.id==null)
                {
                    emp = new employe();
                    utilisateur user = new utilisateur();
                    emp.utilisateur = user;
                    emp.utilisateur.mot_passe = this.RandomString();
                }
                else
                {
                   emp = cnx.employe.Where(e => e.id_employe == userview.id).Single();
                }
                emp.updateFromView(userview);
                emp.utilisateur.UpdateFromView(userview);
                if (emp.isUnique())
                {
                    if (userview.id == null)
                    {
                        cnx.employe.AddObject(emp);
                        cnx.SaveChanges();
                        // on envoie un email à l'employé
                        EmailInfoConnexion email = new EmailInfoConnexion(emp);
                        new MailController().BienvenueEmail(email).Deliver();
                    }
                    else
                    {
                        cnx.ObjectStateManager.ChangeObjectState(emp, EntityState.Modified);
                        cnx.SaveChanges();
                    }
                    return Json(new { succes = 1 });
                }
                else
                {
                    ModelState.AddModelError("", "Votre identifiant doit être unique");
                }

            }
            return Json(new { succes = 0, erreurs = ModelState.ListeErreurs() });
        }



        /// <summary>
        /// Met un utilisateur en z_actif=false, en soft delete
        /// </summary>
        [RulesManager("can_manage_employes")]
        [HttpPost]
        public ActionResult Effacer(int id)
        {
            
                var employe = cnx.employe.Where(u => u.id_employe == id).Single();
                employe.z_actif = false;
                cnx.SaveChanges();
                return Json(new { succes = 1 });

        }


        /// <summary>
        /// réceptionne le formulaire de changement de mot de passe et procède à la modif
        /// </summary>
      
        [RulesManager("access")]
        [HttpPost]
        public JsonResult ChangerMotPasse(ChangerMotPasse formChange)
        {
            if (ModelState.IsValid)
            {
                utilisateur utilisateur = this.rulesManager.getCurrentUtilisateur().utilisateur;
                utilisateur user = cnx.utilisateur.Where(u => u.id_utilisateur == utilisateur.id_utilisateur).Single();
                if (formChange.ancienMotPasse == user.mot_passe)
                {
                    user.mot_passe = formChange.nouveauMotPasse;
                    cnx.SaveChanges();
                    return Json(new { succes = 1 });
                }
                else { ModelState.AddModelError("ancienMotPasse", "L'ancien mot de passe ne correspond pas"); }
            }

            return Json(new { succes = 0, erreurs =  ModelState.ListeErreurs()});
        }

        /// <summary>
        /// renvoie le formulaire de changement de mot de passe
        /// </summary>
        [RulesManager("access")]
        [HttpGet]
        public PartialViewResult ChangerMotPasse()
        {
            return PartialView("_ChangerMotPasse");
        }


        /// <summary>
        /// enregistre la changement de tache favori
        /// </summary>
        [RulesManager("access")]
        [HttpPost]
        public JsonResult ChangerTacheFavori(int listeTache)
        {
            int idEmp = this.getCurrentUtilisateur().id_employe;
            employe emp = cnx.employe.Where(e => e.id_employe == idEmp).FirstOrDefault();
            emp.id_tache = listeTache;
            cnx.ObjectStateManager.ChangeObjectState(emp, EntityState.Modified);
            cnx.SaveChanges();
            return Json(new { succes = 1 });
               
        }

        /// <summary>
        /// renvoie le formulaire de changement de mot de passe
        /// </summary>
        [RulesManager("access")]
        [HttpGet]
        public PartialViewResult ChangerTacheFavori()
        {
            int id_emp = this.getCurrentUtilisateur().id_employe;
            int? id_tache = cnx.employe.Where(e => e.id_employe == id_emp).FirstOrDefault().id_tache;
            ViewBag.listeTache = new SelectList(cnx.tache, "id_tache", "nom_tache", id_tache);
            return PartialView("_TacheFavorie");
        }
       
       

    }
}
