using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Words
    {
        public int wordsId { get; set; }
        public int? verseId { get; set; }
        public int? sureId { get; set; }
        public string? wordText { get; set; }
        public string? wordRe { get; set; }
    }
}