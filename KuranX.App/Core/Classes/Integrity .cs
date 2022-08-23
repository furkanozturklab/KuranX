using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Integrity
    {
        public int IntegrityId { get; set; }
        public string? IntegrityName { get; set; }
        public int? connectVerseId { get; set; }
        public int? connectSureId { get; set; }
        public int? connectedVerseId { get; set; }
        public int? connectedSureId { get; set; }
        public string? IntegrityNote { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modify { get; set; } = DateTime.Now;
    }
}