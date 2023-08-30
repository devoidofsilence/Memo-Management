using SBLApps.Models;
using Microsoft.EntityFrameworkCore;

namespace SBLApps.Data
{
    public class MemoAppDbContext : DbContext
    {
        #region Fields
        string connectionString = string.Empty;
        public IConfiguration Configuration { get; }
        #endregion

        #region Ctor
        public MemoAppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration["ConnectionStrings:SBLAppsDbConnection"];
        }
        #endregion

        #region DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleMapper> UserRoleMappers { get; set; }
        public DbSet<MemoType> MemoTypes { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<BlacklistingMemoMain> BlacklistingMemoMains { get; set; }
        public DbSet<BlacklistingMemoDetail> BlacklistingMemoDetails { get; set; }
        public DbSet<BlacklistingOtherPartyDetail> BlacklistingOtherPartyDetails { get; set; }
        public DbSet<BlacklistingDocumentDetail> BlacklistingDocumentDetails { get; set; }

        public DbSet<OtherAccount> OtherAccounts { get; set; }

        public DbSet<LinkedEntitiesDetail> LinkedEntitiesDetails { get; set; }

        //Workflow related
        public DbSet<Operation> Operations { get; set; }
        public DbSet<MemoRequestOperation> MemoRequestOperations { get; set; }
        #endregion

        #region Fluent Builders
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(connectionString);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the entity relationships and constraints here
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => new { e.UserId });
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserRoleId });
                entity.Property(e => e.UserRoleName).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.HasKey(e => new { e.CustomerTypeId });
                entity.Property(e => e.CustomerTypeName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<MemoType>(entity =>
            {
                entity.HasKey(e => new { e.MemoTypeId });
                entity.Property(e => e.MemoTypeName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => new { e.BranchId });
                entity.Property(e => e.BranchName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => new { e.DepartmentId });
                entity.Property(e => e.DepartmentName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<RequestStatus>(entity =>
            {
                entity.HasKey(e => new { e.RequestStatusId });
                entity.Property(e => e.RequestStatusName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasKey(e => new { e.OperationID });
                entity.Property(e => e.OperationName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.OperationCompletedName).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.HasKey(e => new { e.DocumentTypeId });
                entity.Property(e => e.DocumentTypeName).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<BlacklistingMemoMain>()
            .HasIndex(e => e.ReferenceNumber)
            .IsUnique();

            modelBuilder.Entity<BlacklistingMemoMain>(entity =>
            {
                entity.HasKey(e => e.MemoId);
                entity.Property(e => e.MemoId).ValueGeneratedOnAdd();
                entity.Property(e => e.ReferenceNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.FinalApproverSAMName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Subject).HasMaxLength(500);
                entity.Property(e => e.CIF).HasMaxLength(20);
                entity.Property(e => e.AccountNumber).HasMaxLength(20);
                entity.Property(e => e.AccountHolderName).HasMaxLength(100);
                entity.Property(e => e.NameOfRORM).HasMaxLength(100);
                entity.Property(e => e.NameOfPayee).HasMaxLength(100);
                entity.Property(e => e.Initiator).HasMaxLength(100);
                entity.Property(e => e.BlacklistingRequestedBy).HasMaxLength(100);
                entity.Property(e => e.NextAuthority).HasMaxLength(100);
                entity.Property(e => e.MemoRequirementRemarks).HasMaxLength(5000);
                entity.Property(e => e.AddressOfRequestingPerson).HasMaxLength(200);
                entity.Property(e => e.StaffIDNoOfRequestingPerson).HasMaxLength(10);
                entity.Property(e => e.ContactNumberOfRequestingPerson).HasMaxLength(50);

                // Add other property configurations here
                entity.HasOne(e => e.MemoType)
                    .WithMany()
                    .HasForeignKey(e => e.MemoTypeId);

                entity.HasOne(e => e.Branch)
                    .WithMany()
                    .HasForeignKey(e => e.BranchId);

                entity.HasOne(e => e.Department)
                    .WithMany()
                    .HasForeignKey(e => e.DepartmentId);

                entity.HasOne(e => e.RequestStatus)
                    .WithMany()
                    .HasForeignKey(e => e.RequestStatusId);

                entity.HasOne(e => e.Operation)
                    .WithMany()
                    .HasForeignKey(e => e.LatestOperationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BlacklistingMemoDetail>(entity =>
            {
                entity.HasKey(e => e.MemoDetailId);
                entity.Property(e => e.AccountNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.ChequeNumber).HasMaxLength(50);
                entity.Property(e => e.ChequeIssueDate).HasMaxLength(10);
                entity.Property(e => e.ChequeReturnDate).HasMaxLength(200);
                entity.Property(e => e.ReasonOfReturn).HasMaxLength(1000);

                // Add other property configurations here

                entity.HasOne(e => e.BlacklistingMemoMain)
                    .WithMany(m => m.BlacklistingMemoDetails)
                    .HasForeignKey(e => e.MemoId);
            });

            modelBuilder.Entity<BlacklistingOtherPartyDetail>(entity =>
            {
                entity.HasKey(e => e.OtherPartyDetailId);
                entity.Property(e => e.FullName).HasMaxLength(200);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Remarks).HasMaxLength(1000);

                // Add other property configurations here

                entity.HasOne(e => e.BlacklistingMemoMain)
                    .WithMany(m => m.BlacklistingOtherPartyDetails)
                    .HasForeignKey(e => e.MemoId);
            });

            modelBuilder.Entity<OtherAccount>(entity =>
            {
                entity.HasKey(e => e.DetailId);
                entity.Property(e => e.CIF).HasMaxLength(20);
                entity.Property(e => e.AccountNumber).HasMaxLength(20);
                entity.Property(e => e.AccountScheme).HasMaxLength(10);
                entity.Property(e => e.AccountStatus).HasMaxLength(100);
                entity.Property(e => e.FreezeStatus).HasMaxLength(100);
                entity.Property(e => e.Balance).HasMaxLength(50);

                // Add other property configurations here

                entity.HasOne(e => e.BlacklistingMemoMain)
                    .WithMany(m => m.OtherAccounts)
                    .HasForeignKey(e => e.MemoId);
            });

            modelBuilder.Entity<LinkedEntitiesDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId);
                entity.Property(e => e.Cif).HasMaxLength(20);
                entity.Property(e => e.AccountNumber).HasMaxLength(20);
                entity.Property(e => e.AccountName).HasMaxLength(500);
                entity.Property(e => e.MainAccountNumber).HasMaxLength(50);
                entity.Property(e => e.Balance).HasMaxLength(50);
                entity.Property(e => e.AccountStatus).HasMaxLength(100);
                entity.Property(e => e.FreezeStatus).HasMaxLength(100);

                // Add other property configurations here

                entity.HasOne(e => e.BlacklistingMemoMain)
                    .WithMany(m => m.LinkedEntitiesDetails)
                    .HasForeignKey(e => e.MemoId);
            });

            modelBuilder.Entity<MemoRequestOperation>(entity =>
            {
                entity.HasKey(e => new { e.MemoRequestOperationId });
                entity.Property(e => e.OperationBy).HasMaxLength(100);
                entity.Property(e => e.RequestComingFrom).HasMaxLength(100);
                entity.Property(e => e.OperationRemarks).HasMaxLength(5000).IsRequired();
                entity.Property(e => e.BlacklistNumber).HasMaxLength(50);
                entity.Property(e => e.BlacklistDocumentFullPath).HasMaxLength(1000);
            });

            modelBuilder.Entity<MemoRequestOperation>()
                .HasOne(mro => mro.Request)                     // MemoRequestOperation has one BlacklistingMemoMain
                .WithMany()      // BlacklistingMemoMain has many MemoRequestOperation
                .HasForeignKey(mro => mro.RequestID)            // Foreign key property
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemoRequestOperation>()
                .HasOne(no => no.Operation)
                .WithMany()
                .HasForeignKey(no => no.OperationID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
