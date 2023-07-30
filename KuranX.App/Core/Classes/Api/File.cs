
using System.Collections.Generic;


namespace KuranX.App.Core.Classes.Api
{
    public class File
    {

        public int file_id { get; set; }
        public int? project_id { get; set; }
        public string? file_name { get; set; }
        public string? file_size { get; set; }
        public string? file_real_path { get; set; }
        public string? file_hash { get; set; }


        public class RootProjectFile
        {
            public List<File>? Files { get; set; }
        }


        // sql deki tabloların karşılığı olarak table_projectFile
    }
}
