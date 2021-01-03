using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniPrj_1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}