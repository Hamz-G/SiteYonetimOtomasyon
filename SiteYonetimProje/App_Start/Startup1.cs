﻿using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(SiteYonetimProje.App_Start.Startup1))]

namespace SiteYonetimProje.App_Start
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = "ApplicationCookie",
                    LoginPath = new PathString("/Admin/Login")
                });
        }
    }
}
