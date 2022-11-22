﻿// <auto-generated />
using System;
using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KuranX.App.Migrations
{
    [DbContext(typeof(AyetContext))]
    [Migration("20221120161036_MainBuild-Hotfix5")]
    partial class MainBuildHotfix5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("KuranX.App.Core.Classes.Integrity", b =>
                {
                    b.Property<int>("IntegrityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("IntegrityName")
                        .HasColumnType("TEXT");

                    b.Property<string>("IntegrityNote")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modify")
                        .HasColumnType("TEXT");

                    b.Property<int?>("connectSureId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("connectVerseId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("connectedSureId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("connectedVerseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IntegrityId");

                    b.ToTable("Integrity");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Interpreter", b =>
                {
                    b.Property<int?>("interpreterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("interpreterDetail")
                        .HasColumnType("TEXT");

                    b.Property<string>("interpreterWriter")
                        .HasColumnType("TEXT");

                    b.Property<int?>("sureId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("verseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("interpreterId");

                    b.ToTable("Interpreter");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Library", b =>
                {
                    b.Property<int>("LibraryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("LibraryColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("LibraryName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modify")
                        .HasColumnType("TEXT");

                    b.HasKey("LibraryId");

                    b.ToTable("Librarys");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Notes", b =>
                {
                    b.Property<int>("NotesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int?>("LibraryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("Modify")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteDetail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteLocation")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PdfFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int?>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int?>("SureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int?>("VerseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.HasKey("NotesId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.PdfFile", b =>
                {
                    b.Property<int>("PdfFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileSize")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("User");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modify")
                        .HasColumnType("TEXT");

                    b.HasKey("PdfFileId");

                    b.ToTable("PdfFile");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Remider", b =>
                {
                    b.Property<int>("RemiderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConnectSureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConnectVerseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Create")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastAction")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoopType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("False");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(1);

                    b.Property<DateTime>("RemiderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("RemiderDetail")
                        .HasColumnType("TEXT");

                    b.Property<string>("RemiderName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Wait");

                    b.HasKey("RemiderId");

                    b.ToTable("Remider");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Result", b =>
                {
                    b.Property<int>("ResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResultFinallyNote")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Sonuç Metninizi buraya yaza bilirsiniz.");

                    b.Property<bool?>("ResultLib")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("ResultName")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("ResultNotes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("ResultStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("#ADB5BD");

                    b.Property<bool?>("ResultSubject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.HasKey("ResultId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.ResultItem", b =>
                {
                    b.Property<int>("ResultItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ResultId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ResultLibId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("ResultNoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("ResultSubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("SendTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2022, 11, 20, 19, 10, 36, 742, DateTimeKind.Local).AddTicks(3223));

                    b.HasKey("ResultItemId");

                    b.ToTable("ResultItems");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modify")
                        .HasColumnType("TEXT");

                    b.Property<string>("SubjectColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("SubjectName")
                        .HasColumnType("TEXT");

                    b.HasKey("SubjectId");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.SubjectItems", b =>
                {
                    b.Property<int>("SubjectItemsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modify")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SubjectName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SubjectNotesId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SureId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VerseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SubjectItemsId");

                    b.ToTable("SubjectItems");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Sure", b =>
                {
                    b.Property<int?>("sureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Complated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DeskLanding")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DeskList")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int?>("DeskMushaf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LandingLocation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumberOfVerses")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserCheckCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int?>("UserLastRelativeVerse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.HasKey("sureId");

                    b.ToTable("Sure");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Tasks", b =>
                {
                    b.Property<int>("TasksId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MissonsColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MissonsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MissonsRepeart")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MissonsTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MissonsType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TasksId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.User", b =>
                {
                    b.Property<int?>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.Property<string>("AvatarUrl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("default")
                        .HasColumnName("user_avatarUrl");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("user_createDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("user_email");

                    b.Property<string>("FirstName")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("First Name")
                        .HasColumnName("user_fisrtName");

                    b.Property<string>("LastName")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Last Name")
                        .HasColumnName("user_lastName");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT")
                        .HasColumnName("user_password");

                    b.Property<string>("ScreetQuestion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Screet Question")
                        .HasColumnName("user_screetQuestion");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("user_updateDate");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Verse", b =>
                {
                    b.Property<int?>("VerseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("MarkCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<int?>("RelativeDesk")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("RemiderCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VerseArabic")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("VerseCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("VerseDesc")
                        .HasColumnType("TEXT");

                    b.Property<string>("VerseTr")
                        .HasColumnType("TEXT");

                    b.HasKey("VerseId");

                    b.ToTable("Verse");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Words", b =>
                {
                    b.Property<int>("WordsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SureId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VerseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WordRe")
                        .HasColumnType("TEXT");

                    b.Property<string>("WordText")
                        .HasColumnType("TEXT");

                    b.HasKey("WordsId");

                    b.ToTable("Words");
                });
#pragma warning restore 612, 618
        }
    }
}
