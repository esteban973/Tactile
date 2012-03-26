using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_HELIANTIS.Models;
using System.IO;
using CRM_HELIANTIS.Utils;
using Microsoft.Reporting.WebForms;


namespace CRM_HELIANTIS.Classes
{
    public class BaseController : Controller
    {
        public RulesManager rulesManager { get; set; }

        public RulesManager RulesManager
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    
        protected HeliantisEntities cnx;

        /// <summary>
        /// Crée une chaine de caractères au hasard pour générer un mot de passe
        /// </summary>

        protected string RandomString()
        {
            string password = "";
            Random random = new Random();
            char ch;
            for (int i = 0; i < 8; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                password = password + ch;
            }
            return password;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            this.cnx = new HeliantisEntities();
            this.rulesManager = new RulesManager();
            this.rulesManager.setNewContext(requestContext.HttpContext);
            base.Initialize(requestContext);
        }



        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = this.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(this.ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #region reporting
        public FileContentResult ReportViewRenderPdf(string ReportDir, string DataSourceName, object datas, string filename)
        {
            return this.ReportViewRender(ReportDir, DataSourceName, datas, "PDF", filename);
        }

        public FileContentResult ReportViewRenderPdf(string ReportDir, Dictionary<String, Object> listdatasource, string filename)
        {
            return this.ReportViewRender(ReportDir, listdatasource, "PDF", filename);
        }

        public FileContentResult ReportViewRenderExcel(string ReportDir, string DataSourceName, object datas, string filename)
        {
            return this.ReportViewRender(ReportDir, DataSourceName, datas, "Excel", filename);
        }

        public FileContentResult ReportViewRenderExcel(string ReportDir, Dictionary<String, Object> listdatasource, string filename)
        {
            return this.ReportViewRender(ReportDir, listdatasource, "Excel", filename);
        }

        public FileContentResult ReportViewRenderWord(string ReportDir, string DataSourceName, object datas, string filename)
        {
            return this.ReportViewRender(ReportDir, DataSourceName, datas, "Word", filename);
        }

        protected FileContentResult ReportViewRender(string ReportDir, string DataSourceName, object datas, string TypeExport, string filename)
        {
            LocalReport localReport = new LocalReport();

            localReport.ReportPath = ReportDir;

            ReportDataSource reportDataSource = new ReportDataSource(DataSourceName, datas);
            localReport.DataSources.Add(reportDataSource);


            string reportType = TypeExport;
            string mimeType;
            string encoding;
            string fileNameExtension;


            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + TypeExport + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";



            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType, filename);
        }


        protected FileContentResult ReportViewRender(string ReportDir, Dictionary<String, Object> listdatasource, string TypeExport, string filename)
        {
            LocalReport localReport = new LocalReport();

            localReport.ReportPath = ReportDir;

            foreach (var datasource in listdatasource)
            {
                ReportDataSource reportDataSource = new ReportDataSource(datasource.Key, datasource.Value);
                localReport.DataSources.Add(reportDataSource);
            }



            string reportType = TypeExport;
            string mimeType;
            string encoding;
            string fileNameExtension;


            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + TypeExport + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";



            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType, filename);
        }


        #endregion




    } // fin classe

} // fin namespace