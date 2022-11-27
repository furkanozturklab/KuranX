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
        public string integrityName { get; set; } = "Default";
        public int connectVerseId { get; set; } = 0;
        public int connectSureId { get; set; } = 0;
        public int connectedVerseId { get; set; } = 0;
        public int connectedSureId { get; set; } = 0;
        public string integrityNote { get; set; } = "Default";

        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}