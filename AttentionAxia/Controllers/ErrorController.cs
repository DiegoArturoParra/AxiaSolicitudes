using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string error)
        {
            return View();
        }

        public ActionResult Status404()
        {
            return View();
        }
    }
}