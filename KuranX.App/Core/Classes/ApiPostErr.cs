using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class ApiPostErr : Post
    {

        public List<ErrorMessage> Data { get; set; }
    }
}
