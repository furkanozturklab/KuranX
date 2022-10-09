using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Remider
    {
        public int RemiderId { get; set; }
        public string RemiderName { get; set; }
        public string RemiderDetail { get; set; }
        public string LoopType { get; set; }
        public string Status { get; set; }

        public DateTime RemiderDate { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public DateTime LastAction { get; set; } = DateTime.Now;
        public int ConnectVerseId { get; set; }
        public int ConnectSureId { get; set; }
        public int Priority { get; set; }
    }
}