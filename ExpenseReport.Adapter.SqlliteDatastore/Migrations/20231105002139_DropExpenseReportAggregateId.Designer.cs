﻿// <auto-generated />
using System;
using Application.Adapter;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    [DbContext(typeof(ExpensesDbContext))]
    [Migration("20231105002139_DropExpenseReportAggregateId")]
    partial class DropExpenseReportAggregateId
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

                    b.Property<int>("ExpenseReportAggregateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExpenseType")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ExpensesReportAggregateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ExpensesReportAggregateId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Application.Services.ExpensesReportAggregate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ExpenseReportAggregates");
                });

            modelBuilder.Entity("Application.Services.Expenses", b =>
                {
                    b.HasOne("Application.Services.ExpensesReportAggregate", null)
                        .WithMany("Expenses")
                        .HasForeignKey("ExpensesReportAggregateId");
                });

            modelBuilder.Entity("Application.Services.ExpensesReportAggregate", b =>
                {
                    b.Navigation("Expenses");
                });
#pragma warning restore 612, 618
        }
    }
}