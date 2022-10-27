using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Library
    {
        public int LibraryId { get; set; }
        public string? LibraryName { get; set; }
        public string? LibraryColor { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modify { get; set; } = DateTime.Now;
    }
}