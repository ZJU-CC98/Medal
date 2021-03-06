﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CC98.Medal.Data;

namespace CC98.Medal.Migrations
{
    [DbContext(typeof(MedalDataModel))]
    [Migration("20160701081351_UpdatePriceSettings")]
    partial class UpdatePriceSettings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CC98.Medal.Data.Medal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CanApply");

                    b.Property<int?>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("ImageUri")
                        .IsRequired();

                    b.Property<string>("Link");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int?>("Price");

                    b.Property<string>("PriceSettingsInternal")
                        .HasColumnName("PriceSettings");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Medals");
                });

            modelBuilder.Entity("CC98.Medal.Data.MedalCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.ToTable("MedalCategories");
                });

            modelBuilder.Entity("CC98.Medal.Data.UserMedalIssue", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("MedalId");

                    b.Property<bool>("NeedReview");

                    b.Property<DateTime>("Time");

                    b.HasKey("UserId", "MedalId");

                    b.HasIndex("MedalId");

                    b.ToTable("UserMedalIssues");
                });

            modelBuilder.Entity("CC98.Medal.Data.Medal", b =>
                {
                    b.HasOne("CC98.Medal.Data.MedalCategory", "Category")
                        .WithMany("Medals")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("CC98.Medal.Data.UserMedalIssue", b =>
                {
                    b.HasOne("CC98.Medal.Data.Medal", "Medal")
                        .WithMany()
                        .HasForeignKey("MedalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
