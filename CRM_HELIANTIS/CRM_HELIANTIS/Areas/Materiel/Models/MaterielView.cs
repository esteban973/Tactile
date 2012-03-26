using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_HELIANTIS.Models;
using System.ComponentModel.DataAnnotations;

namespace CRM_HELIANTIS.Areas.Materiel.Models
{
    public class MaterielView
    {
        [Required(ErrorMessage = "Vous devez renseigner le nom du matériel")]
        public string nom { get; set; }
        public int id { get; set; }

        public void updateFromModel(materiel matos)
        {
            this.nom = matos.nom;
            this.id = matos.id_materiel;
        }

    }


}
