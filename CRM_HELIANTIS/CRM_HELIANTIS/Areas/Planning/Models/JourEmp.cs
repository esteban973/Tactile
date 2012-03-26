using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Planning.Models
{
    public class JourEmp
    {
        public DateTime debutJournee { get; set; }
        public DateTime finJournee { get; set; }
        public List<TempsJournee> tpsJour { get; set; }

        public JourEmp(VuePlanningGlobal vpg)
        {
            this.DetermineLeJour(vpg.date_debut);
            this.tpsJour = new List<TempsJournee>();
            this.ajouteTempsJournee(vpg);
        }

        public void DetermineLeJour(DateTime deb)
        {
            this.debutJournee = new DateTime(deb.Year, deb.Month, deb.Day, 5, 0, 0);
            this.finJournee = new DateTime(deb.Year, deb.Month, deb.Day, 20, 0, 0);
        }

        public object retourEnJson()
        {
            return new
            {
                from = this.debutJournee,
                to = this.finJournee,
                desc = this.creerDescription(),
                customClass = this.determineClasse()
            };
        }

        public string creerDescription()
        {
            string description = "";
            foreach (var tps in this.tpsJour)
            {
                description = description + tps.description + "<br/>";
            }
            return description;
        }

        public string determineClasse()
        {
            int type = this.tpsJour.FirstOrDefault().idtypetemps;
            foreach (var tps in this.tpsJour)
            {
                if (type != tps.idtypetemps) return "classeMix";
            }
            return "classe" + type;
        }

        public void ajouteTempsJournee(VuePlanningGlobal vpg)
        {
            TempsJournee tpsJournee = new TempsJournee(vpg);
            this.tpsJour.Add(tpsJournee);
        }

    }
}
