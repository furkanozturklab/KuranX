using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class SubjectItems
    {
        public int subjectItemsId { get; set; }
        public int? subjectId { get; set; }
        public int? sureId { get; set; }
        public int? verseId { get; set; }
        public int? subjectNotesId { get; set; }
        public string? subjectName { get; set; }
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}