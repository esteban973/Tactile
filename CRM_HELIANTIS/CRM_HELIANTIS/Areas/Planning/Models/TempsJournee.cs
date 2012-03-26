using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_HELIANTIS.Models;

namespace CRM_HELIANTIS.Areas.Planning.Models
{
    public class TempsJournee
    {
        public int idtypetemps { get; set; }
        public string description { get; set; }
        public string nomtypetemps { get; set; }

        public TempsJournee(VuePlanningGlobal vpg)
        {
            this.idtypetemps = vpg.id_type_temps;
            this.description = " de " + vpg.date_debut.ToShortTimeString() +
                                " à " + vpg.date_fin.ToShortTimeString() +
                                " - <strong>" + vpg.typetemps + "</strong>" +
                                " " + vpg.nomclient;
            this.nomtypetemps = vpg.typetemps;
        }
    }
}
