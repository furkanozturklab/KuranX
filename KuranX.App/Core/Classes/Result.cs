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
        public int resultId { get; set; }
        public string resultName { get; set; }
        public bool resultLib { get; set; }
        public bool resultNotes { get; set; }
        public bool resultSubject { get; set; }
        public string resultStatus { get; set; }
        public string resultFinallyNote { get; set; }
    }
}