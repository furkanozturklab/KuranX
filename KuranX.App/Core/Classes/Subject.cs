using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Subject
    {
        public int SubjectId { get; set; }

        public string? SubjectName { get; set; }
        public string? SubjectColor { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modify { get; set; } = DateTime.Now;
    }
}