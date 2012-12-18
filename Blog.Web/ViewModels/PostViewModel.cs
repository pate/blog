using System;
using System.Web.Mvc;

namespace Blog.Web.ViewModels
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public string Slug { get; set; }

        public MvcHtmlString Body { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
    }
}
