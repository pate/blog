using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Areas.Admin.Models
{
    public class ViewTemplateInputModel
    {
        public string ViewPath { get; set; }

        [DataType(DataType.MultilineText)]
        public string ViewData { get; set; }
    }
}