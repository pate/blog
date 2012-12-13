using System;
using DreamSongs.MongoRepository;

namespace Blog.Data.Models
{
    [Serializable]
    public class Role : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}