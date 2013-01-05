using System;
using System.ComponentModel.DataAnnotations;
using DreamSongs.MongoRepository;

namespace Blog.Data.Models
{
    public enum PostFormat
    {
        Plaintext,
        Html,
        Markdown
    }

    public class Post : Entity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Slug { get; set; }

        public string ImageUrl { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }

        public PostFormat Format { get; set; } // Markdown, 

        public bool IsActive { get; set; }
    }
}
