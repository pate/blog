using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Blog.Data.Models;
using Blog.Web.ViewModels;
using DreamSongs.MongoRepository;
using MarkdownDeep;
using PagedList;

namespace Blog.Web.Controllers
{
    public class PostController : Controller
    {
        private IRepository<Post> Posts { get; set; }
        private Markdown Markdown { get; set; }

        public PostController(IRepository<Post> postRepo)
        {
            Posts = postRepo;
            Markdown = new Markdown()
                           {
                               ExtraMode = true
                           };
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
                                            ImageUrl = p.ImageUrl,
                                            PublishedOn = p.PublishedOn
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

            if (post.Format == PostFormat.Markdown)
            {
                // Translate
                var html = Markdown.Transform(post.Body);
                post.Body = html; // not great
            }

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
