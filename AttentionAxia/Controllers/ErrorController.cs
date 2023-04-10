using System;
using System.Net.NetworkInformation;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string ErrorMessage, Exception Exception)
        {
            if (!IsInternetConnected())
            {
                ViewBag.ErrorCode = "No se pudo establecer una conexión a Internet";
            }
            else
            {
                ViewData["ErrorMessage"] = ErrorMessage;
            }
            return View();
        }

        public ActionResult Status404()
        {
            return View();
        }
        public static bool IsInternetConnected()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var result = ping.Send("www.google.com");
                    return (result.Status == IPStatus.Success);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}