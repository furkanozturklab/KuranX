using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Result
    {
        public int ResultId { get; set; }
        public string ResultName { get; set; }
        public bool ResultLib { get; set; }
        public bool ResultNotes { get; set; }
        public bool ResultSubject { get; set; }
        public string ResultStatus { get; set; }
        public string ResultFinallyNote { get; set; }
    }
}