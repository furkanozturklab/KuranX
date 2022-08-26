using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class SubjectItems
    {
        public int SubjectItemsId { get; set; }
        public int? SubjectId { get; set; }
        public int? SureId { get; set; }
        public int? VerseId { get; set; }
        public int? SubjectNotesId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modify { get; set; } = DateTime.Now;
    }
}