﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RetroPieRomUploader.Data;

namespace RetroPieRomUploader.Migrations
{
    [DbContext(typeof(RetroPieRomUploaderContext))]
    [Migration("20210615105320_RomMultipleFiles")]
    partial class RomMultipleFiles
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("RetroPieRomUploader.Models.ConsoleType", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ConsoleType");
                });

            modelBuilder.Entity("RetroPieRomUploader.Models.Rom", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConsoleTypeID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ConsoleTypeID");

                    b.ToTable("Rom");
                });

            modelBuilder.Entity("RetroPieRomUploader.Models.RomFileEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RomID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("RomID");

                    b.ToTable("RomFileEntry");
                });

            modelBuilder.Entity("RetroPieRomUploader.Models.Rom", b =>
                {
                    b.HasOne("RetroPieRomUploader.Models.ConsoleType", "ConsoleType")
                        .WithMany("Roms")
                        .HasForeignKey("ConsoleTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConsoleType");
                });

            modelBuilder.Entity("RetroPieRomUploader.Models.RomFileEntry", b =>
                {
                    b.HasOne("RetroPieRomUploader.Models.Rom", "Rom")
                        .WithMany("FileEntries")
                        .HasForeignKey("RomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rom");
                });

            modelBuilder.Entity("RetroPieRomUploader.Models.ConsoleType", b =>
                {
                    b.Navigation("Roms");
                });

            modelBuilder.Entity("RetroPieRomUploader.Models.Rom", b =>
                {
                    b.Navigation("FileEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
