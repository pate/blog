using System.IO;
using System.Web.Hosting;

namespace Blog.Web.Infrastructure
{
    public class CustomVirtualFile : VirtualFile
    {
        private readonly string _viewData;
        public CustomVirtualFile(string virtualPath, string viewData) : base(virtualPath)
        {
            this._viewData = viewData;
        }

        public override Stream Open()
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(_viewData));
        }
    }
}