
using System.Collections.Generic;


namespace KuranX.App.Core.Classes.Api
{
    public class ApiProject : Post
    {

        public List<Project> Data { get; set; }

    }

    // burda yapı post kalıtım alarak ona ait olan message ve code alıp birde data ekliyor burdaki data aldığım tablo daki veriye göre sekillenir. List<Project> seklinde alıyorum.
}
