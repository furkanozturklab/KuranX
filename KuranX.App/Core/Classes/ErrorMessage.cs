using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class ErrorMessage
    {

        public int project_id { get; set; }

        public string project_errDate { get; set; }

        public string project_errDetail { get; set; }

        public string project_fileLocation { get; set; }

        public string project_Code { get; set; }

        public class ErrorMessageRoot
        {
            public List<ErrorMessage>? errorMessages { get; set; }
        }

    }
}
