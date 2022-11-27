using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Interpreter
    {
        public int interpreterId { get; set; }
        public int verseId { get; set; } = 0;
        public int sureId { get; set; } = 0;
        public string interpreterWriter { get; set; } = "Default";
        public string interpreterDetail { get; set; } = "Default";
    }
}