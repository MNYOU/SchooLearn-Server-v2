﻿// <auto-generated />
using System;
using Dal.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dal.EFCore.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230507120719_GroupStudentConf")]
    partial class GroupStudentConf
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.2.23128.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dal.Entities.Admin", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("InstitutionId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.ToTable("admins", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("ApplicationResult")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("result");

                    b.Property<long>("InstitutionId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsReviewed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_reviewed");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionId");

                    b.ToTable("applications", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.Difficulty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<byte>("Scores")
                        .HasColumnType("smallint")
                        .HasColumnName("scores");

                    b.HasKey("Id");

                    b.ToTable("difficulties", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.FileData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("content");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_type");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.HasKey("Id");

                    b.ToTable("file_data", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<long>("InvitationCode")
                        .HasColumnType("bigint")
                        .HasColumnName("invitation_code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeacherId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InvitationCode")
                        .IsUnique();

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("group", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.GroupStudent", b =>
                {
                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsApproved")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_approved");

                    b.HasKey("GroupId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("GroupStudents");
                });

            modelBuilder.Entity("Dal.Entities.Institution", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AdminId")
                        .HasColumnType("bigint");

                    b.Property<long?>("InvitationCodeForTeachers")
                        .HasColumnType("bigint")
                        .HasColumnName("invitation_code_for_teachers");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_confirmed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("PrimaryInvitationCode")
                        .HasColumnType("bigint");

                    b.Property<long>("TIN")
                        .HasMaxLength(12)
                        .HasColumnType("bigint")
                        .HasColumnName("tin")
                        .IsFixedLength();

                    b.Property<string>("WebAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AdminId")
                        .IsUnique();

                    b.ToTable("institutions", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.SolvedTask", b =>
                {
                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.Property<string>("Answer")
                        .HasColumnType("text")
                        .HasColumnName("answer");

                    b.Property<long>("FileAnswerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("boolean");

                    b.Property<float>("Scores")
                        .HasColumnType("real");

                    b.Property<DateTime>("SolveTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("solve_time");

                    b.HasKey("StudentId", "TaskId");

                    b.HasIndex("FileAnswerId")
                        .IsUnique();

                    b.HasIndex("TaskId");

                    b.ToTable("solved_tasks", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.Student", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long?>("InstitutionId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_confirmed");

                    b.HasKey("UserId");

                    b.HasIndex("InstitutionId");

                    b.ToTable("students", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.Subject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("TeacherId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("subjects", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.Task", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("answer");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_datetime");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("execution_period");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<long>("DifficultyId")
                        .HasColumnType("bigint");

                    b.Property<long>("InstitutionId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsExtended")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeacherId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DifficultyId");

                    b.HasIndex("InstitutionId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("tasks", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.Teacher", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("InstitutionId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.HasIndex("InstitutionId");

                    b.ToTable("teachers", (string)null);
                });

            modelBuilder.Entity("Dal.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("nickname");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<short>("Role")
                        .HasColumnType("smallint")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("GroupTask", b =>
                {
                    b.Property<long>("GroupsId")
                        .HasColumnType("bigint");

                    b.Property<long>("TasksId")
                        .HasColumnType("bigint");

                    b.HasKey("GroupsId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("GroupTask");
                });

            modelBuilder.Entity("Dal.Entities.Admin", b =>
                {
                    b.HasOne("Dal.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("Dal.Entities.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dal.Entities.Application", b =>
                {
                    b.HasOne("Dal.Entities.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Institution");
                });

            modelBuilder.Entity("Dal.Entities.Group", b =>
                {
                    b.HasOne("Dal.Entities.Subject", "Subject")
                        .WithMany("Groups")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Teacher", "Teacher")
                        .WithMany("Groups")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Dal.Entities.GroupStudent", b =>
                {
                    b.HasOne("Dal.Entities.Group", "Group")
                        .WithMany("GroupsStudent")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Student", "Student")
                        .WithMany("GroupsStudent")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Dal.Entities.Institution", b =>
                {
                    b.HasOne("Dal.Entities.Admin", "Admin")
                        .WithOne("Institution")
                        .HasForeignKey("Dal.Entities.Institution", "AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Dal.Entities.SolvedTask", b =>
                {
                    b.HasOne("Dal.Entities.FileData", "FileAnswer")
                        .WithOne()
                        .HasForeignKey("Dal.Entities.SolvedTask", "FileAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Student", "Student")
                        .WithMany("SolvedTasks")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Task", "Task")
                        .WithMany("SolvedTasks")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileAnswer");

                    b.Navigation("Student");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Dal.Entities.Student", b =>
                {
                    b.HasOne("Dal.Entities.Institution", "Institution")
                        .WithMany("Students")
                        .HasForeignKey("InstitutionId");

                    b.HasOne("Dal.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("Dal.Entities.Student", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Institution");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dal.Entities.Subject", b =>
                {
                    b.HasOne("Dal.Entities.Teacher", "Teacher")
                        .WithMany("Subjects")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Dal.Entities.Task", b =>
                {
                    b.HasOne("Dal.Entities.Difficulty", "Difficulty")
                        .WithMany("Tasks")
                        .HasForeignKey("DifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Subject", "Subject")
                        .WithMany("Tasks")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Difficulty");

                    b.Navigation("Institution");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Dal.Entities.Teacher", b =>
                {
                    b.HasOne("Dal.Entities.Institution", "Institution")
                        .WithMany("Teachers")
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("Dal.Entities.Teacher", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Institution");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GroupTask", b =>
                {
                    b.HasOne("Dal.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dal.Entities.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dal.Entities.Admin", b =>
                {
                    b.Navigation("Institution")
                        .IsRequired();
                });

            modelBuilder.Entity("Dal.Entities.Difficulty", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Dal.Entities.Group", b =>
                {
                    b.Navigation("GroupsStudent");
                });

            modelBuilder.Entity("Dal.Entities.Institution", b =>
                {
                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("Dal.Entities.Student", b =>
                {
                    b.Navigation("GroupsStudent");

                    b.Navigation("SolvedTasks");
                });

            modelBuilder.Entity("Dal.Entities.Subject", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Dal.Entities.Task", b =>
                {
                    b.Navigation("SolvedTasks");
                });

            modelBuilder.Entity("Dal.Entities.Teacher", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Subjects");
                });
#pragma warning restore 612, 618
        }
    }
}
