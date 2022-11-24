using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Session
    {
        public int sessionId { get; set; }
        public int? userID { get; set; }
        public int? readVerseCount { get; set; }
        public int? currentSureId { get; set; }
        public int? currentVerseId { get; set; }
    }
}