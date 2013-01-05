using System;

namespace Blog.Web.ViewModels
{
    public class PostListViewModel
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? PublishedOn { get; set; } 
    }
}