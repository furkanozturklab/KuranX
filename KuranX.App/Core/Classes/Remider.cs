using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Remider
    {
        public int remiderId { get; set; }
        public string? remiderName { get; set; }
        public string? remiderDetail { get; set; }
        public string? loopType { get; set; }
        public string? status { get; set; }

        public DateTime remiderDate { get; set; }
        public DateTime create { get; set; } = DateTime.Now;
        public DateTime lastAction { get; set; } = DateTime.Now;
        public int connectVerseId { get; set; }
        public int connectSureId { get; set; }
        public int priority { get; set; }
    }
}