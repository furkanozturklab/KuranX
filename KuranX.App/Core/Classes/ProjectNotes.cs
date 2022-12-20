using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class ProjectNotes
    {
        public string project_id { get; set; }
        public string project_version { get; set; }
        public string project_UpdateNote { get; set; }
        public string project_platform { get; set; }
    }

    public class ProjectNotesRoot
    {
        public List<ProjectNotes>? projectnotes { get; set; }
    }
}