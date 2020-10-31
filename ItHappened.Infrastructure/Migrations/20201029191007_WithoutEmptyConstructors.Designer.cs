﻿// <auto-generated />
using System;
using ItHappened.Infrastructure.EFCoreRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItHappened.Infrastructure.Migrations
{
    [DbContext(typeof(ItHappenedDbContext))]
    [Migration("20201029191007_WithoutEmptyConstructors")]
    partial class WithoutEmptyConstructors
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ItHappened.Domain.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ItHappened.Domain.GeoTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("GpsLat")
                        .HasColumnType("float");

                    b.Property<double>("GpsLng")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("GeoTags");
                });
#pragma warning restore 612, 618
        }
    }
}
