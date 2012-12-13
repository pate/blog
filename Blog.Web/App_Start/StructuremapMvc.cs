using System.Web.Mvc;
using Blog.Web.DependencyResolution;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Blog.Web.App_Start.StructuremapMvc), "Start")]

namespace Blog.Web.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            var container = (IContainer) IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}