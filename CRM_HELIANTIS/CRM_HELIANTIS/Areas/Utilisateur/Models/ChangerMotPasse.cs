using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CRM_HELIANTIS.Areas.Utilisateur.Models
{
    public class ChangerMotPasse
    {
        [Required(ErrorMessage="Vous devez renseigner le mot de passe")]
        [DataType(DataType.Password)]
        public string ancienMotPasse { get; set; }
        [Required(ErrorMessage = "Vous devez renseigner le mot de passe")]
        [StringLength(100, ErrorMessage = "Votre mot de passe doit contenir au moins 6 caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string nouveauMotPasse { get; set; }
        [DataType(DataType.Password)]
        [Compare("nouveauMotPasse", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
        public string copieNouveauMotPasse { get; set; }
    }
}