using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Post
    {
        public string? message { get; set; }
        public string? code { get; set; }
    }

    public class RootPost
    {
        public List<Post>? post { get; set; }
    }
}