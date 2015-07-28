using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortfolioSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return RedirectToAction("Index", "Store");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Joe";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Joe";

            return View();
        }

        public ActionResult ImageEditor()
        {
            ViewBag.Message = "Image Editor";

            return View();
        }
    }
}