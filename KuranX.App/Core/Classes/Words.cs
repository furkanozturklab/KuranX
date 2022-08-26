using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Words
    {
        public int WordsId { get; set; }
        public int? VerseId { get; set; }
        public int? SureId { get; set; }
        public string? WordText { get; set; }
        public string? WordRe { get; set; }
    }
}