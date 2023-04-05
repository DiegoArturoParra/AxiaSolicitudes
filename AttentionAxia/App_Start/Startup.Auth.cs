using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Web.Configuration;


namespace AttentionAxia
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = "AXIASOLICITUDES",
                LogoutPath = new PathString("/Account/Logout"),
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromHours(8),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                AuthenticationMode = (Microsoft.Owin.Security.AuthenticationMode)AuthenticationMode.None
            });
        }
    }
}