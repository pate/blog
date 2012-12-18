using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Web.Helpers;

namespace Blog.Web
{
    public enum Roles
    {
        [StringValue("Administrator")] Administrator
    }


    public class Constants
    {
        public class Areas
        {
            public const string Admin = "Admin";
        }

        
    }
}