﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using miniweb.Cloudflare.Api.Data;

#nullable disable

namespace miniweb.Cloudflare.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221113162632_Renaming")]
    partial class Renaming
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.11");

            modelBuilder.Entity("miniweb.Cloudflare.Api.Entities.DnsRecordEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RecordType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ZoneEntityId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ZoneEntityId");

                    b.ToTable("dns_records");
                });

            modelBuilder.Entity("miniweb.Cloudflare.Api.Entities.ZoneEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("zone");
                });

            modelBuilder.Entity("miniweb.Cloudflare.Api.Entities.DnsRecordEntity", b =>
                {
                    b.HasOne("miniweb.Cloudflare.Api.Entities.ZoneEntity", "ZoneEntity")
                        .WithMany()
                        .HasForeignKey("ZoneEntityId");

                    b.Navigation("ZoneEntity");
                });
#pragma warning restore 612, 618
        }
    }
}