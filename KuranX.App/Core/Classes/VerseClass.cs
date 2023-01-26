using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class VerseClass
    {
        public int verseClassId { get; set; }

        public int sureId { get; set; } = 0;
        public int relativeDesk { get; set; } = 0;

        public bool v_hk { get; set; } = true;
        public bool v_tv { get; set; } = false;
        public bool v_cz { get; set; } = false;
        public bool v_mk { get; set; } = false;
        public bool v_du { get; set; } = false;
        public bool v_hr { get; set; } = false;
        public bool v_sn { get; set; } = false;
    }
}