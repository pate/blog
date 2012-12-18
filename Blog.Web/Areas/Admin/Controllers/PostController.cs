using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Blog.Data.Models;
using Blog.Web.Areas.Admin.Models;
using Blog.Web.Helpers;
using Blog.Web.ViewModels;
using DreamSongs.MongoRepository;
using PagedList;

namespace Blog.Web.Areas.Admin.Controllers
{
    public class PostController : AdminController
    {
        private IRepository<Post> Posts { get; set; }

        public PostController(IRepository<Post> postRepo)
        {
            Posts = postRepo;
        }

        public ActionResult Index(
            string q,
            int page = 1,
            int pageSize = 20
            )
        {
            var pages = Posts.All().ToPagedList(page, pageSize);

            return View(pages);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new PostInputModel()
                            {
                                IsActive = true
                            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PostInputModel inputModel)
        {
            // first check if slug exists
            if (Posts.Exists(x => x.Slug == inputModel.Slug && x.IsActive))
                ModelState.AddModelError("Slug", "Post slug must be unique.");

            if (ModelState.IsValid)
            {
                var post = new Post();
                Mapper.Map(inputModel, post);

                post.CreatedOn = DateTime.Now;

                if (post.IsActive)
                    post.PublishedOn = DateTime.Now;

                post = Posts.Add(post);

                this.FlashInfo("Published \"{0}\" on {1}".Fmt(post.Title, post.PublishedOn));

                return RedirectToAction("Display", new { slug = post.Slug, area = "" });
            }

            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var post = Posts.GetById(id);
            if (post == null)
                return HttpNotFound("no such post");

            var viewModel = Mapper.Map<PostViewModel>(post);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var post = Posts.GetById(id);
            if (post == null)
                return HttpNotFound("no such post");

            var inputModel = Mapper.Map<PostInputModel>(post);

            return View(inputModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, PostInputModel inputModel)
        {
            var post = Posts.GetById(id);
            if (post == null)
                return HttpNotFound("no such post");

            // check if slug exists
            if (Posts.Exists(x => x.Slug == inputModel.Slug && x.IsActive && x.Id != id))
                ModelState.AddModelError("Slug", "Post slug must be unique.");

            if (ModelState.IsValid)
            {
                Mapper.Map(inputModel, post);
                post.UpdatedOn = DateTime.Now;
                Posts.Update(post);

                this.FlashInfo("Updated page.");

                return RedirectToAction("Display", new { controller = "Post", area = string.Empty, slug = post.Slug });
            }

            return View(inputModel);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var post = Posts.GetById(id);
            if (post == null)
                return HttpNotFound("no such post");

            var viewModel = Mapper.Map<PostViewModel>(post);

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var post = Posts.GetById(id);
            if (post == null)
                return HttpNotFound("no such post");

            Posts.Delete(post);

            this.FlashInfo("Deleted Post " + post.Id);

            return RedirectToAction("Index");
        }
    }
}
