using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net.Mvc;
using CRM_HELIANTIS.Areas.Utilisateur.Models;

namespace CRM_HELIANTIS.Areas.Utilisateur.Controllers
{
    public class MailController : MailerBase
    {
        /// <summary>
        /// Envoi un email de confirmation à l'utilisateur 
        /// avec ses infos de connexion, le login et mot de passe
        /// </summary>

        public EmailResult BienvenueEmail(EmailInfoConnexion info)
        {
            To.Add(info.email);
            From = "no-reply@heliantis.fr";
            Subject = "Tactile : vos infos de connexion";
            return Email("BienvenueEmail", info);
        }

        /// <summary>
        /// Envoi un email dans le cas d'un oubli de mot de passe
        /// avec ses infos de connexion, le login et mot de passe
        /// </summary>

        public EmailResult MotPassePerdu(EmailInfoConnexion info)
        {
            To.Add(info.email);
            From = "no-reply@heliantis.fr";
            Subject = "Tactile : vos infos de connexion";
            return Email("MotPassePerdu", info);
        }

        /// <summary>
        /// Envoi un email dans le cas d'un oubli de mot de passe
        /// avec ses infos de connexion, le login et mot de passe
        /// </summary>

        public EmailResult Test()
        {
            To.Add("stephane.lanjard@gmail.com");
            From = "no-reply@heliantis.fr";
            Subject = "Tactile : vos infos de connexion";
            return Email("Test");
        }

    }
}
