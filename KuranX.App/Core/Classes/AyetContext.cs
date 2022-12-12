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
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KuranSunnetullah");
            Directory.CreateDirectory(folder);
            DbPath = Path.Combine(folder, "Ayet.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //   USER
            modelBuilder.Entity<User>()
                .Property(u => u.userId)
                .HasColumnName("user_id");
            modelBuilder.Entity<User>()
                .Property(u => u.email)
                .HasColumnName("user_email")
                .HasDefaultValue("none")
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.firstName)
                .HasColumnName("user_firstName")
                .HasDefaultValue("First Name");
            modelBuilder.Entity<User>()
                .Property(u => u.lastName)
                .HasColumnName("user_lastName")
                .HasDefaultValue("Last Name");

            modelBuilder.Entity<User>()
                .Property(u => u.createDate)
                .HasColumnName("user_createDate")
                .HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<User>()
                .Property(u => u.updateDate)
                .HasColumnName("user_updateDate")
                .HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<User>()
                .Property(u => u.avatarUrl)
                .HasColumnName("user_avatarUrl")
                .HasDefaultValue("default");
            modelBuilder.Entity<User>()
               .Property(u => u.screetQuestion)
               .HasColumnName("user_screetQuestion")
               .HasDefaultValue("Değiştiriniz");
            modelBuilder.Entity<User>()
               .Property(u => u.screetQuestionAnw)
               .HasColumnName("user_screetAnw")
               .HasDefaultValue("Yeni Değeri Girin");

            //   SURE
            modelBuilder.Entity<Sure>()
                .Property(u => u.userCheckCount)
                .HasDefaultValue(0);
            modelBuilder.Entity<Sure>()
                .Property(u => u.deskList)
                .HasDefaultValue(0);
            modelBuilder.Entity<Sure>()
                .Property(u => u.userLastRelativeVerse)
                .HasDefaultValue(0);
            modelBuilder.Entity<Sure>()
                .Property(u => u.complated)
                .HasDefaultValue(false);

            //   VERSE
            modelBuilder.Entity<Verse>()
                .Property(u => u.remiderCheck)
                .HasDefaultValue(false);
            modelBuilder.Entity<Verse>()
                .Property(u => u.markCheck)
                .HasDefaultValue(false);
            modelBuilder.Entity<Verse>()
                .Property(u => u.verseCheck)
                .HasDefaultValue(false);
            modelBuilder.Entity<Verse>()
               .Property(u => u.commentary)
               .HasDefaultValue("Wait");

            //   NOTES
            modelBuilder.Entity<Notes>()
                .Property(u => u.sureId)
                .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
                .Property(u => u.verseId)
                .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
               .Property(u => u.subjectId)
               .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
               .Property(u => u.pdfFileId)
               .HasDefaultValue(0);
            modelBuilder.Entity<Notes>()
               .Property(u => u.libraryId)
               .HasDefaultValue(0);

            // INTEGRİTY
            modelBuilder.Entity<Integrity>()
                .Property(u => u.integrityProtected)
                .HasDefaultValue(0);

            // PDF
            modelBuilder.Entity<PdfFile>()
                .Property(u => u.fileType)
                .HasDefaultValue("User");

            // REMIDER

            modelBuilder.Entity<Remider>()
                .Property(u => u.loopType)
                .HasDefaultValue("False");

            modelBuilder.Entity<Remider>()
               .Property(u => u.status)
               .HasDefaultValue("Wait");
            modelBuilder.Entity<Remider>()
              .Property(u => u.priority)
              .HasDefaultValue(1);

            // RESULT
            modelBuilder.Entity<Result>()
                .Property(u => u.resultNotes)
                .HasDefaultValue(false);
            modelBuilder.Entity<Result>()
                .Property(u => u.resultSubject)
                .HasDefaultValue(false);
            modelBuilder.Entity<Result>()
                .Property(u => u.resultLib)
                .HasDefaultValue(false);
            modelBuilder.Entity<Result>()
                .Property(u => u.resultFinallyNote)
                .HasDefaultValue("Sonuç Metninizi buraya yaza bilirsiniz.");

            // RESULT ITEMS
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.resultNoteId)
                .HasDefaultValue(0);
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.resultLibId)
                .HasDefaultValue(0);
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.resultSubjectId)
                .HasDefaultValue(0);
            modelBuilder.Entity<ResultItem>()
                .Property(u => u.sendTime)
                .HasDefaultValue(DateTime.Now);
        }
    }
}