using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Sure
    {
        public int sureId { get; set; }
        public string name { get; set; } = "Default";
        public int numberOfVerses { get; set; } = 0;
        public int numberOfSection { get; set; } = 0;
        public int userCheckCount { get; set; } = 0;
        public int userLastRelativeVerse { get; set; } = 0;
        public string landingLocation { get; set; } = "Default";
        public int deskLanding { get; set; } = 0;
        public int deskMushaf { get; set; } = 0;
        public int deskList { get; set; } = 0;
        public string status { get; set; } = "Default";
        public string description { get; set; } = "Wait";
        public bool completed { get; set; } = false;
    }
}