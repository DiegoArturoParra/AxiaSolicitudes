using AttentionAxia.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    public class BaseController : Controller
    {
        #region Creacion de carpeta si no existe
        public void FolderIsExist(string path)
        {
            bool folderExists = Directory.Exists(Server.MapPath(path));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(path));
        }

        #endregion

        #region Guardar cookies owin del user
        public void SaveCookies(string user, string rol, string id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.Authentication, id)
            };
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
        }
        #endregion

        #region Alerts para mensajes 
        public void SetMessage(string message)
        {
            TempData[GetConstants.MESSAGE] = message;
        }

        public void SetAlert(string alert)
        {
            TempData[GetConstants.ALERT] = alert;
        }
        public void DeleteTempData()
        {
            TempData.Clear();
        }

        #endregion


        #region Cerrar Sesión
        [HttpPost]
        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                Session.Clear();
                Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Response.Cookies.Clear();
                HttpCookie c = new HttpCookie("Login");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                Session.Abandon();
            }
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}