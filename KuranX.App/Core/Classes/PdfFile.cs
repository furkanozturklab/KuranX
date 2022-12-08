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
        public string fileName { get; set; } = "Default";
        public string fileUrl { get; set; } = "Default";
        public string fileSize { get; set; } = "Default";
        public string fileType { get; set; } = "Default";
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}