using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class Project
    {
        public string? project_id { get; set; }
        public string? project_name { get; set; }
        public string? project_createDate { get; set; }
        public string? project_updateDate_x86 { get; set; }
        public string? project_updateDate_x64 { get; set; }
        public string? project_version_x86 { get; set; }
        public string? project_version_x64 { get; set; }
        public string? project_configuration { get; set; }
        public string? project_platform { get; set; }
        public string? project_totalfile { get; set; }
        public string? project_downloadUrl { get; set; }
    }

    public class ProjectRoot
    {
        public List<Project>? project { get; set; }
    }
}