
using System.Collections.Generic;


namespace KuranX.App.Core.Classes.Api
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


    // api dönderdiği json karşılamak için oluşturulan class message ve code hepsinde aynı olduğu için base class bu .
}
