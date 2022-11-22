using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

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
        public DbSet<PdfFile> PdfFile { get; set; }
        public DbSet<Remider> Remider { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<ResultItem> ResultItems { get; set; }
        public DbSet<Library> Librarys { get; set; }

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
            //   USER
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

            //   SURE
            modelBuilder.Entity<Sure>()
                .Property(u => u.UserCheckCount)
                .HasDefaultValue(0);
            modelBuilder.Entity<Sure>()
                .Property(u => u.DeskList)
                .HasDefaultValue(0);
            modelBuilder.Entity<Sure>()
                .Property(u => u.UserLastRelativeVerse)
                .HasDefaultValue(0);
            modelBuilder.Entity<Sure>()
                .Property(u => u.Complated)
                .HasDefaultValue(false);

            //   VERSE
            modelBuilder.Entity<Verse>()
                .Property(u => u.RemiderCheck)
                .HasDefaultValue(false);
            modelBuilder.Entity<Verse>()
                .Property(u => u.MarkCheck)
                .HasDefaultValue(false);
            modelBuilder.Entity<Verse>()
                .Property(u => u.VerseCheck)
                .HasDefaultValue(false);

            //   NOTES
            modelBuilder.Entity<Notes>()
                .Property(u => u.SureId)
                .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
                .Property(u => u.VerseId)
                .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
               .Property(u => u.SubjectId)
               .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
               .Property(u => u.PdfFileId)
               .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
               .Property(u => u.LibraryId)
               .HasDefaultValue(0);

            // PDF
            modelBuilder.Entity<PdfFile>()
                .Property(u => u.FileType)
                .HasDefaultValue("User");

            // REMIDER

            modelBuilder.Entity<Remider>()
                .Property(u => u.LoopType)
                .HasDefaultValue("False");

            modelBuilder.Entity<Remider>()
               .Property(u => u.Status)
               .HasDefaultValue("Wait");
            modelBuilder.Entity<Remider>()
              .Property(u => u.Priority)
              .HasDefaultValue(1);

            // RESULT
            modelBuilder.Entity<Result>()
                .Property(u => u.ResultNotes)
                .HasDefaultValue(false);
            modelBuilder.Entity<Result>()
                .Property(u => u.ResultSubject)
                .HasDefaultValue(false);
            modelBuilder.Entity<Result>()
                .Property(u => u.ResultLib)
                .HasDefaultValue(false);

            modelBuilder.Entity<Result>()
               .Property(u => u.ResultStatus)
               .HasDefaultValue("#ADB5BD");
            modelBuilder.Entity<Result>()
                .Property(u => u.ResultFinallyNote)
                .HasDefaultValue("Sonuç Metninizi buraya yaza bilirsiniz.");

            // RESULT ITEMS
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.ResultNoteId)
                .HasDefaultValue(0);
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.ResultLibId)
                .HasDefaultValue(0);
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.ResultSubjectId)
                .HasDefaultValue(0);
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.SendTime)
                .HasDefaultValue(DateTime.Now);
        }
    }
}