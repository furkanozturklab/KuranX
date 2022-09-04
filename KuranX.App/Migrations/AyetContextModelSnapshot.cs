﻿// <auto-generated />
using System;
using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KuranX.App.Migrations
{
    [DbContext(typeof(AyetContext))]
    partial class AyetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

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

            modelBuilder.Entity("KuranX.App.Core.Classes.Notes", b =>
                {
                    b.Property<int>("NotesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modify")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteDetail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteHeader")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteLocation")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("#ADB5BD");

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
                        .HasDefaultValue(1);

                    b.HasKey("sureId");

                    b.ToTable("Sure");
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

                    b.Property<string>("MarkCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("false");

                    b.Property<int?>("RelativeDesk")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RememberCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("false");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VerseArabic")
                        .HasColumnType("TEXT");

                    b.Property<string>("VerseCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("false");

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
