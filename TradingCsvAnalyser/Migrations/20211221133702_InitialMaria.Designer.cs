﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradingCsvAnalyser.Models.Database;

#nullable disable

namespace TradingCsvAnalyser.Migrations
{
    [DbContext(typeof(AnalyserContext))]
    [Migration("20211221133702_InitialMaria")]
    partial class InitialMaria
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TradingCsvAnalyser.Models.PriceEntry", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset>("DateAndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Symbol", "DateAndTime");

                    b.ToTable("PriceEntries");
                });
#pragma warning restore 612, 618
        }
    }
}