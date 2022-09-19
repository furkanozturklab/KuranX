using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Notes
    {
        public int NotesId { get; set; }
        public int? VerseId { get; set; }
        public int? SureId { get; set; }
        public int? SubjectId { get; set; }
        public int? PdfFileId { get; set; }
        public int? PdfPageId { get; set; }
        public string? NoteHeader { get; set; }
        public string? NoteDetail { get; set; }
        public string? NoteLocation { get; set; }
        public string? NoteStatus { get; set; }
        public string? NoteLibHeader { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modify { get; set; } = DateTime.Now;
    }
}