using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZL.Infrastructure;
namespace ZL.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var controller = RouteData.Values["controller"].ToString();
            Log log = new Log();
            log.Info("老王");
            ZLHttpRequet zlhttp = new ZLHttpRequet();
            string s= zlhttp.Post("http://www.jumax-sh.dev.sudaotech.com/api/mall/auth/login", "{'userName':'15821820391','password':'cjs6666667'}");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}