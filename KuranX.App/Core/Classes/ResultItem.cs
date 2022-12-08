using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class ResultItem
    {
        public int resultItemId { get; set; }
        public int resultId { get; set; } = 0;
        public int resultSubjectId { get; set; } = 0;
        public int resultLibId { get; set; } = 0;
        public int resultNoteId { get; set; } = 0;
        public DateTime sendTime { get; set; } = DateTime.Now;
    }
}