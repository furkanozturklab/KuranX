
using System.Collections.Generic;


namespace KuranX.App.Core.Classes.Api
{
    public class UpdateNote
    {
        public int? update_id { get; set; }
        public int? project_id { get; set; }
        public string? update_date { get; set; }
        public string? update_detail { get; set; }
        public string? update_version { get; set; }



    }

    public class UpdateNoteRoot
    {
        public List<UpdateNote>? updateNote { get; set; }
    }
}
