using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Blog.Data.Models;
using DreamSongs.MongoRepository;

namespace Blog.Web.Infrastructure
{
    public class MongoVirtualPathProvider : VirtualPathProvider
    {
        protected readonly IRepository<ViewTemplate> Views;

        public MongoVirtualPathProvider()
        {
            Views = DependencyResolver.Current.GetService<IRepository<ViewTemplate>>();
        }

        public override bool FileExists(string virtualPath)
        {
            var page = Views.All().FirstOrDefault(x => x.ViewPath == virtualPath);
            return page != null || base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var view = Views.All().FirstOrDefault(x => x.ViewPath == virtualPath);
            
            if (view != null)
                return new CustomVirtualFile(virtualPath, view.ViewData);
            
            return base.GetFile(virtualPath);
        }
    }
}