using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Section
    {


        public int SectionId { get; set; }

        public int SureId { get; set; }

        public int startVerse { get; set; }

        public int endVerse { get; set; }

        public string SectionName { get; set; }

        public string SectionDescription { get; set; }

        public string SectionDetail { get; set; }

        public bool IsMark { get; set; }

        public int SectionNumber { get; set; }

    }
}
