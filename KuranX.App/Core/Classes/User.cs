using System;


namespace KuranX.App.Core.Classes
{
    public class User
    {
        public int userId { get; set; }
        public string email { get; set; } = "none";
        public string firstName { get; set; } = "First Name";
        public string lastName { get; set; } = "Last Name";
        public string pin { get; set; } = "";
        public string screetQuestion { get; set; } = "Değiştiriniz";
        public string screetQuestionAnw { get; set; } = "Yeni Değeri Girin";

        public DateTime createDate { get; set; } = DateTime.Now;

        public DateTime updateDate { get; set; } = DateTime.Now;
        public string avatarUrl { get; set; } = "profile_1";
    }
}