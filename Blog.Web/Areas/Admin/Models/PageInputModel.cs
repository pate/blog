using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Areas.Admin.Models
{
    public class PageInputModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public string ViewPath { get; set; }

        public bool IsActive { get; set; }
    }
}