
using System;


namespace KuranX.App.Core.Classes
{
    public class Subject
    {
        public int subjectId { get; set; }

        public string subjectName { get; set; } = "Default";
        public string subjectColor { get; set; } = "Default";

        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}