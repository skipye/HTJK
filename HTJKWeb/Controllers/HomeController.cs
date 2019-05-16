using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceProject;
using System.Web;

namespace HTJKWeb.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ChinaService CSer = new ChinaService();
        public ActionResult Index()
        {
            var Usert=Request["t"];
            if (!string.IsNullOrEmpty(Usert))
            {
                Session["t"] = Usert;
            }
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult _RightTopNav()
        {
            return View();
        }
        public ActionResult PDropdownlist(int? areaParentId)
        {
            var models = CSer.GetPDropdownlist(areaParentId);
            return Json(models);
        }
        public ActionResult CDropdownlist(int? areaParentId)
        {
            string models = CSer.GetCoption(areaParentId).ToString();
            return Content(models);
        }
        public ActionResult ADropdownlist(int? areaParentId, int? Id)
        {
            string models = CSer.GetAoption(areaParentId).ToString();
            return Content(models);
        }
    }
}
