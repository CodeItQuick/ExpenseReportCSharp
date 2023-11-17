﻿// <auto-generated />
using System;
using Application.Adapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    [DbContext(typeof(ExpensesDbContext))]
    [Migration("20231104211417_CreateExpenseReportForeignKeyReference")]
    partial class CreateExpenseReportForeignKeyReference
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("Application.Services.Expenses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExpenseReportId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExpenseType")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ExpensesReportId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ExpensesReportId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Application.Services.ExpensesReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ExpenseReports");
                });

            modelBuilder.Entity("Application.Services.Expenses", b =>
                {
                    b.HasOne("Application.Services.ExpensesReport", null)
                        .WithMany("Expenses")
                        .HasForeignKey("ExpensesReportId");
                });

            modelBuilder.Entity("Application.Services.ExpensesReport", b =>
                {
                    b.Navigation("Expenses");
                });
#pragma warning restore 612, 618
        }
    }
}
