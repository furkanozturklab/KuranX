using System;


namespace KuranX.App.Core.Classes
{
    public class Remider
    {
        public int remiderId { get; set; }
        public string remiderName { get; set; } = "Default";
        public string remiderDetail { get; set; } = "Default";
        public string loopType { get; set; } = "Default";
        public string status { get; set; } = "Default";
        public DateTime remiderDate { get; set; }
        public DateTime create { get; set; } = DateTime.Now;
        public DateTime lastAction { get; set; } = DateTime.Now;
        public int connectVerseId { get; set; } = 0;
        public int connectSureId { get; set; } = 0;
        public int priority { get; set; } = 0;
    }
}