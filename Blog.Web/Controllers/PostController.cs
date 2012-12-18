using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Blog.Data.Models;
using Blog.Web.ViewModels;
using DreamSongs.MongoRepository;
using PagedList;

namespace Blog.Web.Controllers
{
    public class PostController : Controller
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
            var posts = Posts.All();

            if (!string.IsNullOrEmpty(q))
                posts = posts.Where(x => x.Body.Contains(q));

            var pagedPosts = from p in posts
                             where p.IsActive
                             select new PostListViewModel
                                        {
                                            Id = p.Id,
                                            Slug = p.Slug,
                                            Title = p.Title,
                                            Description = p.Description,
                                            ImageUrl = p.ImageUrl
                                        };

            var viewModel = pagedPosts.ToPagedList(page, pageSize);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Display(string slug)
        {
            var post = Posts.All().FirstOrDefault(x => x.Slug == slug);
            if (post == null)
                return HttpNotFound("no such page");

            var viewModel = Mapper.Map<PostViewModel>(post);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Permalink(string id, string title)
        {
            var post = Posts.GetById(id);
            if (post == null)
                return HttpNotFound("no such page");

            var viewModel = Mapper.Map<PostViewModel>(post);

            return View("Display", viewModel);
        }

    }
}
