using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using AutoMapper;
using Blog.Data.Models;
using Blog.Web.Helpers;
using Blog.Web.ViewModels;
using DreamSongs.MongoRepository;
using PagedList;

namespace Blog.Web.Controllers
{
    public class PostController : Controller
    {
        private IRepository<Page> Pages { get; set; }

        public PostController(IRepository<Page> pageRepo)
        {
            Pages = pageRepo;
        }

        public ActionResult Index(
            string q,
            int page = 1,
            int pageSize = 20
            )
        {
            var pages = Pages.All();

            if (!string.IsNullOrEmpty(q))
                pages = pages.Where(x => x.Body.Contains(q));

            var pagedPages = from p in pages
                             where p.IsActive
                             select new PageListViewModel
                                        {
                                            Id = p.Id,
                                            Slug = p.Slug,
                                            Title = p.Title,
                                            Description = p.Description,
                                            ImageUrl = p.ImageUrl
                                        };

            var viewModel = pagedPages.ToPagedList(page, pageSize);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Display(string slug)
        {
            var page = Pages.All().FirstOrDefault(x => x.Slug == slug);
            if (page == null)
                return HttpNotFound("no such page");

            var viewModel = Mapper.Map<PageViewModel>(page);

            if (page.ViewPath.IsEmpty())
                return View(viewModel);

            return View(page.ViewPath, viewModel);
            //return View(System.IO.Path.GetFileNameWithoutExtension(res.ViewName));
        }

        [HttpGet]
        public ActionResult Permalink(string id, string title)
        {
            var page = Pages.GetById(id);
            if (page == null)
                return HttpNotFound("no such page");

            var viewModel = Mapper.Map<PageViewModel>(page);

            return View("View", viewModel);
        }

    }
}
