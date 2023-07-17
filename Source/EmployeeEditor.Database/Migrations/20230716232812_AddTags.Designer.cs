﻿// <auto-generated />
using EmployeeEditor.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeEditor.Database.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    [Migration("20230716232812_AddTags")]
    partial class AddTags
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("EmployeeEditor.Database.Entity.EmployeeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surename")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EmployeeEditor.Database.Entity.EmployeeTag", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TagsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StudentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EmployeeId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("EmployeeTag");
                });

            modelBuilder.Entity("EmployeeEditor.Database.Entity.TagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("EmployeeEditor.Database.Entity.EmployeeTag", b =>
                {
                    b.HasOne("EmployeeEditor.Database.Entity.EmployeeEntity", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmployeeEditor.Database.Entity.TagEntity", "Tags")
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}