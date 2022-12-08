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

        public string resultName { get; set; } = "Default";
        public bool resultLib { get; set; } = false;

        public bool resultNotes { get; set; } = false;
        public bool resultSubject { get; set; } = false;
        public string resultStatus { get; set; } = "Default";
        public string resultFinallyNote { get; set; } = "Default";
    }
}