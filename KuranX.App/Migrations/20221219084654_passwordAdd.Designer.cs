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
    [Migration("20221219084654_passwordAdd")]
    partial class passwordAdd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("KuranX.App.Core.Classes.Integrity", b =>
                {
                    b.Property<int>("integrityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("connectSureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("connectVerseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("connectedSureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("connectedVerseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("created")
                        .HasColumnType("TEXT");

                    b.Property<string>("integrityName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("integrityNote")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("integrityProtected")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("modify")
                        .HasColumnType("TEXT");

                    b.HasKey("integrityId");

                    b.ToTable("Integrity");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Interpreter", b =>
                {
                    b.Property<int>("interpreterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("interpreterDetail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("interpreterWriter")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("sureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("verseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("interpreterId");

                    b.ToTable("Interpreter");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Library", b =>
                {
                    b.Property<int>("libraryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("created")
                        .HasColumnType("TEXT");

                    b.Property<string>("libraryColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("libraryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("modify")
                        .HasColumnType("TEXT");

                    b.HasKey("libraryId");

                    b.ToTable("Librarys");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Notes", b =>
                {
                    b.Property<int>("notesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("created")
                        .HasColumnType("TEXT");

                    b.Property<int>("libraryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("modify")
                        .HasColumnType("TEXT");

                    b.Property<string>("noteDetail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("noteHeader")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("noteLocation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("pdfFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("subjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("sureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("verseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.HasKey("notesId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.PdfFile", b =>
                {
                    b.Property<int>("pdfFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("created")
                        .HasColumnType("TEXT");

                    b.Property<string>("fileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("fileSize")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("fileType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("User");

                    b.Property<string>("fileUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("modify")
                        .HasColumnType("TEXT");

                    b.HasKey("pdfFileId");

                    b.ToTable("PdfFile");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Remider", b =>
                {
                    b.Property<int>("remiderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("connectSureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("connectVerseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("create")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("lastAction")
                        .HasColumnType("TEXT");

                    b.Property<string>("loopType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("False");

                    b.Property<int>("priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(1);

                    b.Property<DateTime>("remiderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("remiderDetail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("remiderName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Wait");

                    b.HasKey("remiderId");

                    b.ToTable("Remider");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Result", b =>
                {
                    b.Property<int>("resultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("resultFinallyNote")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Sonuç Metninizi buraya yaza bilirsiniz.");

                    b.Property<bool>("resultLib")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("resultName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("resultNotes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("resultSubject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.HasKey("resultId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.ResultItem", b =>
                {
                    b.Property<int>("resultItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("resultId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("resultLibId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("resultNoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("resultSubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("sendTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(7174));

                    b.HasKey("resultItemId");

                    b.ToTable("ResultItems");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Subject", b =>
                {
                    b.Property<int>("subjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("modify")
                        .HasColumnType("TEXT");

                    b.Property<string>("subjectColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("subjectName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("subjectId");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.SubjectItems", b =>
                {
                    b.Property<int>("subjectItemsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("modify")
                        .HasColumnType("TEXT");

                    b.Property<int>("subjectId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("subjectName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("subjectNotesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("sureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("verseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("subjectItemsId");

                    b.ToTable("SubjectItems");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Sure", b =>
                {
                    b.Property<int>("sureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("complated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("deskLanding")
                        .HasColumnType("INTEGER");

                    b.Property<int>("deskList")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("deskMushaf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("landingLocation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("numberOfVerses")
                        .HasColumnType("INTEGER");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("userCheckCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("userLastRelativeVerse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.HasKey("sureId");

                    b.ToTable("Sure");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Tasks", b =>
                {
                    b.Property<int>("tasksId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("missonsColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("missonsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("missonsRepeart")
                        .HasColumnType("INTEGER");

                    b.Property<int>("missonsTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("missonsType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("tasksId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.Property<string>("avatarUrl")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("default")
                        .HasColumnName("user_avatarUrl");

                    b.Property<DateTime>("createDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(2356))
                        .HasColumnName("user_createDate");

                    b.Property<string>("email")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("none")
                        .HasColumnName("user_email");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("First Name")
                        .HasColumnName("user_firstName");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Last Name")
                        .HasColumnName("user_lastName");

                    b.Property<string>("pin")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("screetQuestion")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Değiştiriniz")
                        .HasColumnName("user_screetQuestion");

                    b.Property<string>("screetQuestionAnw")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Yeni Değeri Girin")
                        .HasColumnName("user_screetAnw");

                    b.Property<DateTime>("updateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(2494))
                        .HasColumnName("user_updateDate");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Userhelp", b =>
                {
                    b.Property<int>("UserhelpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("baseName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("infoImage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("infoName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserhelpId");

                    b.ToTable("UserHelp");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Verse", b =>
                {
                    b.Property<int>("verseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("commentary")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("Wait");

                    b.Property<bool>("markCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<int>("relativeDesk")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("remiderCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<int>("sureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("verseArabic")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("verseCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("verseDesc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("verseTr")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("verseId");

                    b.ToTable("Verse");
                });

            modelBuilder.Entity("KuranX.App.Core.Classes.Words", b =>
                {
                    b.Property<int>("wordsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("arp_read")
                        .HasColumnType("TEXT");

                    b.Property<string>("root")
                        .HasColumnType("TEXT");

                    b.Property<int?>("sureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("tr_read")
                        .HasColumnType("TEXT");

                    b.Property<int?>("verseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("word_meal")
                        .HasColumnType("TEXT");

                    b.HasKey("wordsId");

                    b.ToTable("Words");
                });
#pragma warning restore 612, 618
        }
    }
}
