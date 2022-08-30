using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Session
    {
        public int SessionId { get; set; }
        public int? UserID { get; set; }
        public int? ReadVerseCount { get; set; }
        public int? CurrentSureId { get; set; }
        public int? CurrentVerseId { get; set; }
    }
}