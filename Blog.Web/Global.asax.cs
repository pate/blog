﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Blog.Web.App_Start;
using Blog.Web.Infrastructure;

namespace Blog.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //HostingEnvironment.RegisterVirtualPathProvider(new MongoVirtualPathProvider());
            AreaRegistration.RegisterAllAreas();

            AutoMapperWebConfiguration.Configure();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();

            var nustache = new Nustache.Mvc.NustacheViewEngine();
            ViewEngines.Engines.Add(nustache);

            var razor = new RazorViewEngine();
            ViewEngines.Engines.Add(razor);
        }
    }
}