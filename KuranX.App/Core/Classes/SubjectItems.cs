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
        public int subjectId { get; set; } = 0;
        public int sureId { get; set; } = 0;
        public int verseId { get; set; } = 0;
        public int subjectNotesId { get; set; } = 0;
        public string subjectName { get; set; } = "Default";
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}