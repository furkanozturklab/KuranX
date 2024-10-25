﻿
namespace KuranX.App.Core.Classes
{
    public class Verse
    {
        public int verseId { get; set; }
        public int sureId { get; set; } = 0;
        public int relativeDesk { get; set; } = 0;
        public string verseArabic { get; set; } = "Default";
        public string verseTr { get; set; } = "Default";
        public string verseDesc { get; set; } = "Wait";
        public bool verseCheck { get; set; } = false;
        public bool markCheck { get; set; } = false;
        public bool remiderCheck { get; set; } = false;
    }
}