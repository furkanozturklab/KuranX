﻿using System;


namespace KuranX.App.Core.Classes
{
    public class Notes
    {
        public int notesId { get; set; }
        public int verseId { get; set; } = 0;
        public int sureId { get; set; } = 0;
        public int subjectId { get; set; } = 0;

        public int sectionId { get; set; } = 0;
        public string noteHeader { get; set; } = "Default";
        public string noteDetail { get; set; } = "Default";
        public string noteLocation { get; set; } = "Default";

        
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime modify { get; set; } = DateTime.Now;
    }
}