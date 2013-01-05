using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Areas.Admin.Models
{
    public class PostInputModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Published On")]
        public DateTime? PublishedOn { get; set; }

        [Display(Name = "Published?")]
        public bool IsActive { get; set; }
    }
}