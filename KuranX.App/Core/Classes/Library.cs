using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Library
    {
        public int libraryId { get; set; }
        public string libraryName { get; set; } = "Default";
        public string libraryColor { get; set; } = "#FFFFFF";
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}