using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(ShopBanGiay.Startup1))]

namespace ShopBanGiay
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/User/Login")
            });
        }
    }
}
