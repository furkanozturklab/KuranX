using System;


namespace KuranX.App.Core.Classes
{
    public class Integrity
    {
        public int integrityId { get; set; }

        public string integrityName { get; set; } = "Default";
        public int connectVerseId { get; set; } = 0;
        public int connectSureId { get; set; } = 0;
        public int connectedVerseId { get; set; } = 0;
        public int connectedSureId { get; set; } = 0;
        public string integrityNote { get; set; } = "Default";

        public bool integrityProtected { get; set; } = false;
        public DateTime created { get; set; } = DateTime.Now;

        public DateTime modify { get; set; } = DateTime.Now;
    }
}