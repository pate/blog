using System;
using DreamSongs.MongoRepository;

namespace Blog.Data.Models
{
    public class ViewTemplate : Entity
    {
        public string ViewPath { get; set; }
        public string ViewData { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
