using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Planning.Models
{
    public class LigneGantt
    {
        public int idEmploye;
        public string nomEmploye { get; set; }
        public string nomService { get; set; }
        public List<JourEmp> valeurs { get; set; }

        public LigneGantt(employe emp)
        {
            this.idEmploye = emp.id_employe;
            this.valeurs = new List<JourEmp>();
            this.nomEmploye = emp.prenom + " " + emp.nom.ToUpper();
            this.nomService = emp.groupe.nom;
        }

        public object retourEnJson()
        {
            return new
            {
                name = this.nomEmploye,
                desc = this.nomService,
                values = this.listeJournee()
            };
        }


        public List<object> listeJournee()
        {
            List<object> liste = new List<object>();
            foreach (var val in valeurs)
            {
                liste.Add(val.retourEnJson());
            }
            return liste;
        }

        public void ajouteAJourEmps(VuePlanningGlobal vpg)
        {
            if (this.valeurs.LastOrDefault() == null)
            {
                JourEmp jemp = new JourEmp(vpg);
                this.valeurs.Add(jemp);
            }
            else
            {
                var testDate = this.valeurs.LastOrDefault().debutJournee.ToShortDateString();
                var testDate2 = vpg.date_debut.ToShortDateString();
                if (this.valeurs.LastOrDefault().debutJournee.ToShortDateString() == vpg.date_debut.ToShortDateString())
                {
                    this.valeurs.LastOrDefault().ajouteTempsJournee(vpg);
                }
                else
                {
                    JourEmp jemp = new JourEmp(vpg);
                    this.valeurs.Add(jemp);
                }

            }

        }
    }
}