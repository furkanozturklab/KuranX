using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class PdfFile
    {
        public int PdfFileId { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modify { get; set; } = DateTime.Now;
    }
}