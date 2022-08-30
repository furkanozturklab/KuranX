using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace KuranX.App.Core.Classes
{
    public class AyetContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Sure> Sure { get; set; }
        public DbSet<Verse> Verse { get; set; }
        public DbSet<Interpreter> Interpreter { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Integrity> Integrity { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<SubjectItems> SubjectItems { get; set; }
        public DbSet<Words> Words { get; set; }

        public string DbPath { get; }

        public AyetContext()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KuranX");
            Directory.CreateDirectory(folder);
            DbPath = Path.Combine(folder, "Ayet.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .HasColumnName("user_id");
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnName("user_email")
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .HasColumnName("user_fisrtName")
                .HasDefaultValue("First Name");
            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .HasColumnName("user_lastName")
                .HasDefaultValue("Last Name");
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasColumnName("user_password");
            modelBuilder.Entity<User>()
                .Property(u => u.ScreetQuestion)
                .HasColumnName("user_screetQuestion")
                .HasDefaultValue("Screet Question");
            modelBuilder.Entity<User>()
                .Property(u => u.CreateDate)
                .HasColumnName("user_createDate");
            modelBuilder.Entity<User>()
                .Property(u => u.UpdateDate)
                .HasColumnName("user_updateDate");
            modelBuilder.Entity<User>()
                .Property(u => u.AvatarUrl)
                .HasColumnName("user_avatarUrl")
                .HasDefaultValue("default");

            modelBuilder.Entity<Sure>()
                .Property(u => u.UserCheckCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<Sure>()
                .Property(u => u.UserLastRelativeVerse)
                .HasDefaultValue(1);

            modelBuilder.Entity<Verse>()
                .Property(u => u.RememberCheck)
                .HasDefaultValue("false");
            modelBuilder.Entity<Verse>()
                .Property(u => u.MarkCheck)
                .HasDefaultValue("false");
            modelBuilder.Entity<Verse>()
                .Property(u => u.VerseCheck)
                .HasDefaultValue("false");
        }
    }
}