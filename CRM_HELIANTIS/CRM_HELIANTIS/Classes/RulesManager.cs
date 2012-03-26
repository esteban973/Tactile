#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Models;
using System.Web.Mvc;
#endregion

namespace CRM_HELIANTIS.Classes
{

    #region Extensions permettant le contrôle des droits rapidement dans un contrôleur
    public static class RulesManagerExtensions
    {
        public static void setConnexion(this BaseController ctrl, string identifiant, string mot_passe)
        {
            ctrl.rulesManager.setConnexion(identifiant, mot_passe);
        }

        public static employe getCurrentUtilisateur(this BaseController ctrl)
        {
            return ctrl.rulesManager.getCurrentUtilisateur();
        }

        public static bool isConnected(this BaseController ctrl)
        {
            return ctrl.rulesManager.isConnected();
        }

        public static bool isAuthorized(this BaseController ctrl, string ruleName)
        {
            return ctrl.rulesManager.isAuthorized(ruleName);
        }

        public static void Disconnected(this BaseController ctrl)
        {
            ctrl.rulesManager.Disconnected();
        }

    } // fin classe
    #endregion


    #region Helpers permettant le contrôle des droits rapidement dans une vue
    public static class RulesManagerHelpers
    {

        public static employe getCurrentUtilisateur(this HtmlHelper ctrl)
        {
            return (ctrl.ViewContext.Controller as BaseController).rulesManager.getCurrentUtilisateur();
        }

        public static bool isConnected(this HtmlHelper ctrl)
        {
            return (ctrl.ViewContext.Controller as BaseController).rulesManager.isConnected();
        }

        public static bool isAuthorized(this HtmlHelper ctrl, string ruleName)
        {
            return (ctrl.ViewContext.Controller as BaseController).rulesManager.isAuthorized(ruleName);
        }

    } // fin classe
    #endregion



    /// <summary>
    /// Manager des droits
    /// </summary>
    public class RulesManager
    {
        public HttpContextBase CurrentContext { get; set; }
    
        public void setNewContext(HttpContextBase CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }



        #region création de connexion et infos sur la connexion
        /// <summary>
        /// Crée une connexion à l'application, si les droits et le login/password sont ok
        /// </summary>
        public void setConnexion(string identifiant, string mot_passe)
        {
            using (HeliantisEntities cnx = new HeliantisEntities())
            {
                utilisateur user = cnx.utilisateur.Where(d => d.identifiant == identifiant && d.mot_passe == mot_passe && d.employe.FirstOrDefault().z_actif == true).FirstOrDefault();
                if (user != null)
                {
                    // l'utilisateur existe et sa connexion est ok. 
                    // a t il le droit d'accéder à l'application.
                    employe emp = user.employe.FirstOrDefault();
                    if ((emp != null))
                    {
                        this.CurrentContext.Session["user"] = emp;

                        rule mRule = cnx.rule.Where(d => d.rulename == "access").FirstOrDefault();
                        if (mRule != null)
                        {
                            rule_role mRuleRole = emp.role.rule_role.Where(c => c.id_rule == mRule.id_rule).FirstOrDefault();
                            if (mRuleRole != null)
                            {
                                List<StockRules> RuleRole = (from e1 in cnx.role
                                                             join e2 in cnx.rule_role on e1.id_role equals e2.id_role
                                                             where e1.id_role == emp.id_role
                                                             select new StockRules
                                                             {
                                                                 id_role = e1.id_role,
                                                                 id_rule = (int)e2.id_rule,
                                                                 rulename = e2.rule.rulename
                                                             }).ToList();

                                this.CurrentContext.Session["user_role"] = RuleRole;
                            }
                        }
                    }
                }
            }
        }



        /// <summary>
        /// L'utilisateur est il connecté
        /// </summary>
        /// <returns></returns>
        public bool isConnected()
        {
            if (this.CurrentContext.Session["user"] is employe && this.CurrentContext.Session["user_role"] is List<StockRules>)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Déconnexion de la session courante
        /// </summary>
        public void Disconnected()
        {
            this.CurrentContext.Session.RemoveAll();
            this.CurrentContext.Session.Abandon();
        }
        #endregion


        #region récupération d'informations sur la connexion courante et les droits de la connexion courante
        /// <summary>
        /// Raccourci afin de récupérer l'utilisateur courant
        /// </summary>
        /// <returns></returns>
        public employe getCurrentUtilisateur()
        {
            return this.CurrentContext.Session["user"] as employe;
        }



        /// <summary>
        /// L'utilisateur peut-il exécuter le droit ruleName
        /// </summary>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        public bool isAuthorized(string ruleName)
        {
            HeliantisEntities cnx = new HeliantisEntities();
            {
                List<StockRules> mRole = this.CurrentContext.Session["user_role"] as List<StockRules>;
                if (mRole != null)
                {
                    StockRules mRule = mRole.Where(d => d.rulename == ruleName).FirstOrDefault();

                    if (mRule != null)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }
        #endregion

    } // fin classe


    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RulesManagerAttribute : ActionFilterAttribute
    {
        private readonly string ruleName;

        public RulesManagerAttribute(string ruleName)
        {
            this.ruleName = ruleName;
        }

        #region mise en place de l'attribut à l'exécution
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RulesManager rm = new RulesManager();
            rm.setNewContext(filterContext.HttpContext);

            if (!rm.isConnected())
            {
                //si utilisateur non connecté on le renvoie à la page de login
                filterContext.Result = new HttpUnauthorizedResult("You have to authenticate");
            }
            else
            {
                if (!rm.isAuthorized(this.ruleName))
                {
                    /* possibilité de remplacer par un beau new HttpStatusCodeResult(403);*/
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "ErrorAuthorization",
                        ViewData = filterContext.Controller.ViewData
                    };
                }
            }
        }
        #endregion

    } // fin classe



    public class StockRules
    {
        public int id_role { get; set; }
        public int id_rule { get; set; }
        public string rulename { get; set; }
    }

} // fin namespace