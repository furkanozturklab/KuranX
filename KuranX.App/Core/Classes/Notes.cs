using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Notes
    {
        public int notesId { get; set; }
        public int? verseId { get; set; }
        public int? sureId { get; set; }
        public int? subjectId { get; set; }
        public int? pdfFileId { get; set; }
        public int? libraryId { get; set; }
        public string? noteHeader { get; set; }
        public string? noteDetail { get; set; }
        public string? noteLocation { get; set; }

        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}