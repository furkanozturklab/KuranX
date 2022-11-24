using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Integrity
    {
        public int integrityId { get; set; }
        public string? integrityName { get; set; }
        public int? connectVerseId { get; set; }
        public int? connectSureId { get; set; }
        public int? connectedVerseId { get; set; }
        public int? connectedSureId { get; set; }
        public string? integrityNote { get; set; }

        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}