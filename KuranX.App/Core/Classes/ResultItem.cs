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
        public int resultId { get; set; }
        public int resultSubjectId { get; set; }
        public int resultLibId { get; set; }
        public int resultNoteId { get; set; }
        public DateTime sendTime { get; set; }
    }
}