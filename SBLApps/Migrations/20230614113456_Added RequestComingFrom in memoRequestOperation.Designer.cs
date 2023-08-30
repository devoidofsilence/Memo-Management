﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SBLApps.Data;

#nullable disable

namespace SBLApps.Migrations
{
    [DbContext(typeof(MemoAppDbContext))]
    [Migration("20230614113456_Added RequestComingFrom in memoRequestOperation")]
    partial class AddedRequestComingFrominmemoRequestOperation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SBLApps.Models.BlacklistingCorporateDetail", b =>
                {
                    b.Property<long>("CorporateDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CorporateDetailId"), 1L, 1);

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CompanyFullName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("MemoId")
                        .HasColumnType("bigint");

                    b.Property<string>("Remarks")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("SNo")
                        .HasColumnType("int");

                    b.Property<decimal>("ShareHoldingPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CorporateDetailId");

                    b.HasIndex("MemoId");

                    b.ToTable("BlacklistingCorporateDetails");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingDocumentDetail", b =>
                {
                    b.Property<long>("DocumentDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("DocumentDetailId"), 1L, 1);

                    b.Property<string>("DocumentFullPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<long>("MemoId")
                        .HasColumnType("bigint");

                    b.HasKey("DocumentDetailId");

                    b.HasIndex("MemoId");

                    b.ToTable("BlacklistingDocumentDetails");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingMemoDetail", b =>
                {
                    b.Property<long>("MemoDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MemoDetailId"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("ChequeAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ChequeNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("MemoId")
                        .HasColumnType("bigint");

                    b.Property<string>("ReasonOfReturn")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("SNo")
                        .HasColumnType("int");

                    b.HasKey("MemoDetailId");

                    b.HasIndex("MemoId");

                    b.ToTable("BlacklistingMemoDetails");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingMemoMain", b =>
                {
                    b.Property<long>("MemoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MemoId"), 1L, 1);

                    b.Property<string>("AccountHolderName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("AddressOfRequestingPerson")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("BlacklistingApplicationReceivedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BlacklistingRequestedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<string>("ContactNumberOfRequestingPerson")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CustomerTypeId")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("FinalApproverSAMName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsLoanCustomer")
                        .HasColumnType("bit");

                    b.Property<int>("LatestOperationId")
                        .HasColumnType("int");

                    b.Property<string>("MemoRequirementRemarks")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MemoTypeId")
                        .HasColumnType("int");

                    b.Property<string>("NameOfPayee")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NameOfRORM")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NextAuthority")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ReferenceNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RequestStatusId")
                        .HasColumnType("int");

                    b.Property<string>("StaffIDNoOfRequestingPerson")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("TotalChequeAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalLoanOutstanding")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MemoId");

                    b.HasIndex("BranchId");

                    b.HasIndex("CustomerTypeId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("LatestOperationId");

                    b.HasIndex("MemoTypeId");

                    b.HasIndex("ReferenceNumber")
                        .IsUnique();

                    b.HasIndex("RequestStatusId");

                    b.ToTable("BlacklistingMemoMains");
                });

            modelBuilder.Entity("SBLApps.Models.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BranchId"), 1L, 1);

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BranchId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("SBLApps.Models.CustomerType", b =>
                {
                    b.Property<int>("CustomerTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerTypeId"), 1L, 1);

                    b.Property<string>("CustomerTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CustomerTypeId");

                    b.ToTable("CustomerTypes");
                });

            modelBuilder.Entity("SBLApps.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"), 1L, 1);

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("SBLApps.Models.DocumentType", b =>
                {
                    b.Property<int>("DocumentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentTypeId"), 1L, 1);

                    b.Property<string>("DocumentTypeName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("DocumentTypeId");

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("SBLApps.Models.MemoRequestOperation", b =>
                {
                    b.Property<long>("MemoRequestOperationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MemoRequestOperationId"), 1L, 1);

                    b.Property<string>("OperationBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OperationID")
                        .HasColumnType("int");

                    b.Property<string>("OperationRemarks")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestComingFrom")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<long>("RequestID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("MemoRequestOperationId");

                    b.HasIndex("OperationID");

                    b.HasIndex("RequestID");

                    b.ToTable("MemoRequestOperations");
                });

            modelBuilder.Entity("SBLApps.Models.MemoType", b =>
                {
                    b.Property<int>("MemoTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemoTypeId"), 1L, 1);

                    b.Property<string>("MemoTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MemoTypeId");

                    b.ToTable("MemoTypes");
                });

            modelBuilder.Entity("SBLApps.Models.NextOperationMapper", b =>
                {
                    b.Property<int>("CurrentOperationID")
                        .HasColumnType("int");

                    b.Property<int>("NextOperationID")
                        .HasColumnType("int");

                    b.HasKey("CurrentOperationID", "NextOperationID");

                    b.HasIndex("NextOperationID");

                    b.ToTable("NextOperationMappers");
                });

            modelBuilder.Entity("SBLApps.Models.Operation", b =>
                {
                    b.Property<int>("OperationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OperationID"), 1L, 1);

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("OperationID");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("SBLApps.Models.OtherAccount", b =>
                {
                    b.Property<long>("DetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("DetailId"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("AccountScheme")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("AccountStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Balance")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CIF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FreezeStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("MemoId")
                        .HasColumnType("bigint");

                    b.HasKey("DetailId");

                    b.HasIndex("MemoId");

                    b.ToTable("OtherAccounts");
                });

            modelBuilder.Entity("SBLApps.Models.RequestStatus", b =>
                {
                    b.Property<int>("RequestStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestStatusId"), 1L, 1);

                    b.Property<string>("RequestStatusName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RequestStatusId");

                    b.ToTable("RequestStatuses");
                });

            modelBuilder.Entity("SBLApps.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingCorporateDetail", b =>
                {
                    b.HasOne("SBLApps.Models.BlacklistingMemoMain", "BlacklistingMemoMain")
                        .WithMany("BlacklistingCorporateDetails")
                        .HasForeignKey("MemoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlacklistingMemoMain");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingDocumentDetail", b =>
                {
                    b.HasOne("SBLApps.Models.BlacklistingMemoMain", "BlacklistingMemoMain")
                        .WithMany("BlacklistingDocumentDetails")
                        .HasForeignKey("MemoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlacklistingMemoMain");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingMemoDetail", b =>
                {
                    b.HasOne("SBLApps.Models.BlacklistingMemoMain", "BlacklistingMemoMain")
                        .WithMany("BlacklistingMemoDetails")
                        .HasForeignKey("MemoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlacklistingMemoMain");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingMemoMain", b =>
                {
                    b.HasOne("SBLApps.Models.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SBLApps.Models.CustomerType", "CustomerType")
                        .WithMany()
                        .HasForeignKey("CustomerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SBLApps.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SBLApps.Models.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("LatestOperationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SBLApps.Models.MemoType", "MemoType")
                        .WithMany()
                        .HasForeignKey("MemoTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SBLApps.Models.RequestStatus", "RequestStatus")
                        .WithMany()
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("CustomerType");

                    b.Navigation("Department");

                    b.Navigation("MemoType");

                    b.Navigation("Operation");

                    b.Navigation("RequestStatus");
                });

            modelBuilder.Entity("SBLApps.Models.MemoRequestOperation", b =>
                {
                    b.HasOne("SBLApps.Models.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SBLApps.Models.BlacklistingMemoMain", "Request")
                        .WithMany()
                        .HasForeignKey("RequestID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Operation");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("SBLApps.Models.NextOperationMapper", b =>
                {
                    b.HasOne("SBLApps.Models.Operation", "CurrentOperation")
                        .WithMany()
                        .HasForeignKey("CurrentOperationID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SBLApps.Models.Operation", "NextOperation")
                        .WithMany()
                        .HasForeignKey("NextOperationID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CurrentOperation");

                    b.Navigation("NextOperation");
                });

            modelBuilder.Entity("SBLApps.Models.OtherAccount", b =>
                {
                    b.HasOne("SBLApps.Models.BlacklistingMemoMain", "BlacklistingMemoMain")
                        .WithMany("OtherAccounts")
                        .HasForeignKey("MemoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlacklistingMemoMain");
                });

            modelBuilder.Entity("SBLApps.Models.BlacklistingMemoMain", b =>
                {
                    b.Navigation("BlacklistingCorporateDetails");

                    b.Navigation("BlacklistingDocumentDetails");

                    b.Navigation("BlacklistingMemoDetails");

                    b.Navigation("OtherAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}