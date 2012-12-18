using System;
using System.ComponentModel.DataAnnotations;
using DreamSongs.MongoRepository;

namespace Blog.Data.Models
{
    public class Page : Entity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public string ViewPath { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }

        public bool IsActive { get; set; }
    }
}
