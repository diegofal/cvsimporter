using CsvImporter.Services;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CsvImporter.Web.Controllers
{
    public class HomeController : Controller
    {
        private CsvProcessor csvProcessor;

        public HomeController()
        {
            //TODO: This can be an injected server
            this.csvProcessor = new CsvProcessor();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var filename = WriteFileToFileSystem(file);
            csvProcessor.ProcessCsv(filename);

            ViewBag.UploadSuccess = true;

            return View("Index");
        }

        private string WriteFileToFileSystem(HttpPostedFileBase file)
        {
            string path = String.Empty;

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            return path;
        }
    }
}