﻿// <auto-generated />
using System;
using BankKazungV2.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankKazungV2.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220127184246_FixedMaxLength")]
    partial class FixedMaxLength
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BankKazungV2.Shared.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountID"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(19,4)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("AccountID");

                    b.HasIndex("UserID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.Card", b =>
                {
                    b.Property<int>("CardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardID"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(19,4)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("CreditLimit")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("CardID");

                    b.HasIndex("UserID");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"), 1L, 1);

                    b.Property<int?>("AccountID")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(19,4)");

                    b.Property<int?>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("ReciverAccountID")
                        .HasColumnType("int");

                    b.Property<int>("ReciverCardID")
                        .HasColumnType("int");

                    b.Property<int>("SenderAccountID")
                        .HasColumnType("int");

                    b.Property<int>("SenderCardID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("TransactionID");

                    b.HasIndex("AccountID");

                    b.HasIndex("CardID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.Account", b =>
                {
                    b.HasOne("BankKazungV2.Shared.Models.User", null)
                        .WithMany("Accounts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.Card", b =>
                {
                    b.HasOne("BankKazungV2.Shared.Models.User", null)
                        .WithMany("Cards")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.Transaction", b =>
                {
                    b.HasOne("BankKazungV2.Shared.Models.Account", null)
                        .WithMany("Transactions")
                        .HasForeignKey("AccountID");

                    b.HasOne("BankKazungV2.Shared.Models.Card", null)
                        .WithMany("Transactions")
                        .HasForeignKey("CardID");
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.Card", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("BankKazungV2.Shared.Models.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
