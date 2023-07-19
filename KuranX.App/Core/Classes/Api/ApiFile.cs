using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes.Api
{
    public class ApiFile : Post
    {

        public List<File> Data { get; set; }

    }

    // burda yapı post kalıtım alarak ona ait olan message ve code alıp birde data ekliyor burdaki data aldığım tablo daki veriye göre sekillenir. List<File> seklinde alıyorum.
}
