using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Verses
    {
        public int? versesId { get; set; }
        public string? Name { get; set; }
        public int? NumberOfVerses { get; set; }
        public string? LandingLocation { get; set; }
        public int? DeskLanding { get; set; }
        public int? DeskMushaf { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }
}