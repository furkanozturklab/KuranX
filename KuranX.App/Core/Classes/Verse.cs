using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Verse
    {
        public int? SureId { get; set; }
        public int? VerseId { get; set; }
        public int? RelativeDesk { get; set; }

        public string? VerseArabic { get; set; }
        public string? VerseTr { get; set; }
        public string? VerseDesc { get; set; }
        public string? VerseCheck { get; set; }
        public string? MarkCheck { get; set; }
        public string? RemiderCheck { get; set; }

        public string? Status { get; set; }
    }
}