using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class PdfFile
    {
        public int pdfFileId { get; set; }
        public string fileName { get; set; }
        public string fileUrl { get; set; }
        public string fileSize { get; set; }
        public string fileType { get; set; }
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}