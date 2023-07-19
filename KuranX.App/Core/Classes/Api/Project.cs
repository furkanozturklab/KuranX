using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes.Api
{
    public class Project
    {

        public string? project_id { get; set; }
        public string? project_name { get; set; }
        public string? project_createDate { get; set; }
        public string? project_updateDate { get; set; }
        public string? project_version { get; set; }
        public string? project_configuration { get; set; }
        public string? project_platform { get; set; }

    }

    public class ProjectRoot
    {
        public List<Project>? project { get; set; }
    }

    // sql deki tabloların karşılığı olarak table_project

}
