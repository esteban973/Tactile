using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Classes;
using CRM_HELIANTIS.Models;
using System.Data;
using CRM_HELIANTIS.Areas.Projet.Models;
using CRM_HELIANTIS.Utils;

namespace CRM_HELIANTIS.Areas.Projet.Controllers
{
    public class ClientController : BaseController
    {
        //
        // GET: /Projet/Client/
        [RulesManager("can_manage_clients")]
        public ActionResult Index()
        {
            return View();
        }

        [RulesManager("can_manage_clients")]
        public JsonResult listeClient()
        {
            var clients = cnx.client.Where(c=>c.z_actif==true).OrderBy(c=>c.nom).ToList();
            List<object> retour = new List<object>();
            foreach (var client in clients)
            {
                retour.Add(new
                {
                    id = client.id_client,
                    reference = client.reference,
                    nom = client.nom,
                    bouton = "<span class='edit' data-id=" + client.id_client + ">Editer</span><span class='suppr' data-identite='" + client.nom + "' data-id=" + client.id_client + ">Supprimer</span>"
                });
            }
            return Json(retour, JsonRequestBehavior.AllowGet);
        }

        [RulesManager("can_manage_clients")]
        public PartialViewResult Creer()
        {
            ViewBag.type = "creer";
            ClientView clientview = new ClientView();
            return PartialView("_form", clientview);
        }


        [RulesManager("can_manage_clients")]
        [HttpPost]
        public ActionResult Editer(ClientView clientview, int? id)
        {
           if (ModelState.IsValid)
            {
                if (id == null)
                {
                    client clientx = new client();
                    clientx.updateFromModel(clientview);
                    cnx.client.AddObject(clientx);
                }
                else {
                    client clientx = cnx.client.Where(c => c.id_client == id).Single();
                    clientx.updateFromModel(clientview);
                    cnx.ObjectStateManager.ChangeObjectState(clientx, EntityState.Modified);
                }
                    cnx.SaveChanges();
                    return Json(new { succes = 1 });
            }
            return Json(new { succes = 0,erreurs = ModelState.ListeErreurs()});
        }

        [RulesManager("can_manage_clients")]
        [HttpPost]
        public ActionResult NouveauViaProjet(ClientView clientview)
        {
            if (ModelState.IsValid)
            {
                 client clientx = new client();
                 clientx.updateFromModel(clientview);
                 cnx.client.AddObject(clientx);
                 cnx.SaveChanges();
                 var clients = cnx.client.Where(c => c.z_actif == true);
                 ViewBag.id_client = new SelectList(clients, "id_client", "nom", clientx.id_client);
                 var vue = this.RenderPartialViewToString("_selectClient", null);
                return Json(new { succes = 1 , vue=vue});
            }
            return Json(new { succes = 0, erreurs = ModelState.ListeErreurs() });
        }

        //
        //
        [RulesManager("can_manage_clients")]
        public PartialViewResult Editer(int id)
        {
            ViewBag.type = "editer";
            client client = cnx.client.Single(c => c.id_client == id);
            ClientView clientview = new ClientView();
            clientview.updateFromModel(client);
            return PartialView("_form", clientview);
        }

        

        [RulesManager("can_manage_clients")]
        [HttpPost]
        public ActionResult Effacer(int id)
        {

            var client = cnx.client.Where(c => c.id_client == id).Single();
            client.z_actif = false;
            foreach (var proj in client.projet)
            {
                proj.z_actif = false;
            }
            cnx.ObjectStateManager.ChangeObjectState(client, EntityState.Modified);
            cnx.SaveChanges();
            return Json(new { succes = 1 });
        }

    }
}
