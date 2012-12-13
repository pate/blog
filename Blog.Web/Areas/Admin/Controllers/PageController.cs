using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Blog.Data.Models;
using Blog.Web.Areas.Admin.Models;
using Blog.Web.ViewModels;
using DreamSongs.MongoRepository;
using PagedList;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        private IRepository<Page> Pages { get; set; }

        public PageController(IRepository<Page> pageRepo)
        {
            Pages = pageRepo;
        }

        public ActionResult Index(
            string q,
            int page = 1,
            int pageSize = 20
            )
        {
            var pages = Pages.All().ToPagedList(page, pageSize);

            return View(pages);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new PageInputModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PageInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var page = new Page();
                Mapper.Map(inputModel, page);

                page.CreatedOn = DateTime.Now;
                page = Pages.Add(page);

                return RedirectToAction("Details", new { id = page.Id });
            }

            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var page = Pages.GetById(id);
            if (page == null)
                return HttpNotFound("no such page");

            var viewModel = Mapper.Map<PageViewModel>(page);

            return View(viewModel);
        }

        [HttpGet]
        public new ActionResult View(string slug)
        {
            var page = Pages.All().FirstOrDefault(x => x.Slug == slug);
            if (page == null)
                return HttpNotFound("no such page");

            var viewModel = Mapper.Map<PageViewModel>(page);

            return View(viewModel);
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

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var page = Pages.GetById(id);
            if (page == null)
                return HttpNotFound("no such page");

            var inputModel = Mapper.Map<PageInputModel>(page);

            return View(inputModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, PageInputModel inputModel)
        {
            var page = Pages.GetById(id);
            if (page == null)
                return HttpNotFound("no such page");

            if (ModelState.IsValid)
            {
                Mapper.Map(inputModel, page);
                page.UpdatedOn = DateTime.Now;
                Pages.Update(page);

                this.FlashInfo("Updated page.");

                return RedirectToAction("Display", new { controller = "Post", area = string.Empty, slug = page.Slug });
            }

            return View(inputModel);
        }
    }
}
