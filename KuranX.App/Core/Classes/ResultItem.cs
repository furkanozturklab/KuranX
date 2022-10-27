using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class ResultItem
    {
        public int ResultItemId { get; set; }
        public int ResultId { get; set; }
        public int ResultSubjectId { get; set; }
        public int ResultLibId { get; set; }
        public int ResultNoteId { get; set; }
        public DateTime SendTime { get; set; }
    }
}