using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes
{
    public class User
    {
        public int userId { get; set; }
        public string email { get; set; } = "none";
        public string firstName { get; set; } = "First Name";
        public string lastName { get; set; } = "Last Name";
        public string password { get; set; } = "1230";
        public string screetQuestion { get; set; } = "Screet Question";
        public DateTime createDate { get; set; } = DateTime.Now;
        public DateTime updateDate { get; set; } = DateTime.Now;
        public string avatarUrl { get; set; } = "default";
    }
}