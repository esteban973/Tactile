using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Utilisateur.Controllers
{
    public class RoleController : BaseController
    {
        //TODO ajouter un indice à chacun des listes
        // GET: /Utilisateur/Role/
       [RulesManager("can_manage_roles")]
        public ActionResult Index(string message)
        {
            var ListeRegles = cnx.rule.ToList();
            var ListeRoles = cnx.role.ToList();
            List<object> rolerule = new List<object>();

            foreach (var regle in ListeRegles)
            {
                List<checkBox> retourParRegle = new List<checkBox>();
                //test si le role est attribué à une règle
                foreach (var role in ListeRoles)
                {
                    Boolean check=false;
                    foreach (var roleRegle in regle.rule_role) {
                        if (roleRegle.id_role == role.id_role)
                        {
                            check = true;
                        }
                        if (check==true) break;
                    }

                    retourParRegle.Add(new checkBox(){ 
                        check=check,
                        value=role.id_role,
                        name=regle.id_rule,
                        text=role.role_name
                    });

                }
                rolerule.Add(retourParRegle);
            }
            ViewBag.rolerule = rolerule;
            ViewBag.role = ListeRoles;
            ViewBag.rules = ListeRegles;
            if (message != null) ViewBag.ok = "ok";
            return View("RoleView");
        }

        [RulesManager("can_manage_roles")]
        [HttpPost]
        public ActionResult roleUpdate(FormCollection form)
        {
            //enlever toutes les attributs de roles
            var rule_role = cnx.rule_role.ToList();
            foreach (var rulerole in rule_role)
            {
                cnx.rule_role.DeleteObject(rulerole);

            }
            cnx.SaveChanges();
            //recharger toutes les nouvelles attributions
            foreach (var regle in form.AllKeys)
            {

                string[] rolesId = form[regle].Split(',');
                foreach(var roleId in rolesId){
                    cnx.AddTorule_role(new rule_role { 
                        id_role=Int32.Parse(roleId),
                        id_rule = Int32.Parse(regle)
                    });
                }
                
            }
            cnx.SaveChanges();
            return RedirectToAction("index",new {message="OK"});
        }


       public class checkBox {
        public Boolean check { get; set; }
        public int value { get; set; }
        public int name { get; set; }
       public string text { get; set; } 
       }

    }
}
