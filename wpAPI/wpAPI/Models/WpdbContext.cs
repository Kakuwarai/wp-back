using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace wpAPI.Models;

public partial class WpdbContext : DbContext
{
    public WpdbContext()
    {
    }

    public WpdbContext(DbContextOptions<WpdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Dropdown> Dropdowns { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<StrategicBusinessUnit> StrategicBusinessUnits { get; set; }

    public virtual DbSet<TermsAndCondition> TermsAndConditions { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<Trans3> Trans3s { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    public virtual DbSet<TransactionTermsCondition> TransactionTermsConditions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBranch> UserBranches { get; set; }

    public virtual DbSet<UserMenu> UserMenus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=horreum.ccf9t0eeel7h.ap-southeast-1.rds.amazonaws.com;Database=WPDB;Username=wpuser;Password=wppassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("branch_pk");

            entity.ToTable("Branch");

            entity.HasIndex(e => e.Code, "branch_un").IsUnique();

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Address1).HasColumnType("character varying");
            entity.Property(e => e.Address2).HasColumnType("character varying");
            entity.Property(e => e.Area).HasColumnType("character varying");
            entity.Property(e => e.Code).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Latitude).HasColumnType("character varying");
            entity.Property(e => e.Longitude).HasColumnType("character varying");
            entity.Property(e => e.MapReference).HasColumnType("character varying");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.PostalCode).HasColumnType("character varying");
            entity.Property(e => e.Region).HasColumnType("character varying");
            entity.Property(e => e.ShortName).HasColumnType("character varying");
            entity.Property(e => e.Status).HasDefaultValueSql("1");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pk");

            entity.ToTable("Customer");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Code).HasMaxLength(30);
            entity.Property(e => e.CompanyAddress1)
                .HasMaxLength(255)
                .HasColumnName("Company Address 1");
            entity.Property(e => e.CompanyAddress2)
                .HasMaxLength(255)
                .HasColumnName("Company Address 2");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasColumnName("Company Name");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Position).HasMaxLength(255);
            entity.Property(e => e.Salutation).HasColumnType("character varying");
            entity.Property(e => e.Status).HasDefaultValueSql("true");
        });

        modelBuilder.Entity<Dropdown>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dropdown_pk");

            entity.ToTable("Dropdown");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Code).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Display).HasColumnType("character varying");
            entity.Property(e => e.LookUp).HasColumnType("character varying");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.SortOrder).HasDefaultValueSql("1");
            entity.Property(e => e.Value).HasColumnType("character varying");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("menu_pk");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsBrowser).HasDefaultValueSql("1");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Sort).HasDefaultValueSql("1");
            entity.Property(e => e.Status).HasDefaultValueSql("1");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("position_pk");

            entity.ToTable("Position");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Code).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("true");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rate_pk");

            entity.ToTable("Rate");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Amount)
                .HasPrecision(8, 4)
                .HasDefaultValueSql("0.0000");
            entity.Property(e => e.ChargeCategory).HasColumnType("character varying");
            entity.Property(e => e.ChargeCode).HasColumnType("character varying");
            entity.Property(e => e.ChargeName).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Currency).HasColumnType("character varying");
            entity.Property(e => e.DefaultRemarks).HasColumnType("character varying");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasDefaultValueSql("1");
            entity.Property(e => e.Uom)
                .HasColumnType("character varying")
                .HasColumnName("UOM");
        });

        modelBuilder.Entity<StrategicBusinessUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("strategicbusinessunit_pk");

            entity.ToTable("StrategicBusinessUnit");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Address1).HasColumnType("character varying");
            entity.Property(e => e.Address2).HasColumnType("character varying");
            entity.Property(e => e.Alias).HasColumnType("character varying");
            entity.Property(e => e.Code).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Status).HasDefaultValueSql("1");
        });

        modelBuilder.Entity<TermsAndCondition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("termsandcondition_pk");

            entity.ToTable("TermsAndCondition");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Code).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasDefaultValueSql("1");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tokens_pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.AuthToken).HasColumnType("character varying");
            entity.Property(e => e.ExpiresOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.IssuedOn).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<Trans3>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trans3_pk");

            entity.ToTable("Trans3");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ReferenceInitial)
                .HasMaxLength(30)
                .HasComputedColumnSql("((((\"SBU\")::text || '-'::text) || ((\"YearMonth\")::text || '-'::text)) || lpad((\"Id\")::text, 3, '0'::text))", true);
            entity.Property(e => e.Sbu)
                .HasDefaultValueSql("'FSC'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("SBU");
            entity.Property(e => e.YearMonth)
                .HasDefaultValueSql("to_char(now(), 'YYYY-MM'::text)")
                .HasColumnType("character varying");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_pk");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.BranchCode).HasColumnType("character varying");
            entity.Property(e => e.Commodity).HasColumnType("character varying");
            entity.Property(e => e.CompanyAddress).HasColumnType("character varying");
            entity.Property(e => e.CompanyName).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.CustomerCode).HasColumnType("character varying");
            entity.Property(e => e.CustomerName).HasColumnType("character varying");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ReferenceDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("character varying");
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(30)
                .HasComputedColumnSql("('REF'::text || lpad((\"Id\")::text, 10, '0'::text))", true);
            entity.Property(e => e.Remarks).HasColumnType("character varying");
            entity.Property(e => e.RevisedReference).HasColumnType("character varying");
            entity.Property(e => e.Sbucode)
                .HasColumnType("character varying")
                .HasColumnName("SBUCode");
            entity.Property(e => e.ServiceAddress).HasColumnType("character varying");
            entity.Property(e => e.StorageType).HasColumnType("character varying");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactiondetail_pk");

            entity.ToTable("TransactionDetail");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.ChargeCode).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Currency).HasColumnType("character varying");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Rates).HasColumnType("character varying");
            entity.Property(e => e.ReferenceNumber).HasColumnType("character varying");
            entity.Property(e => e.Remarks).HasColumnType("character varying");
            entity.Property(e => e.Uom)
                .HasColumnType("character varying")
                .HasColumnName("UOM");
        });

        modelBuilder.Entity<TransactionTermsCondition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactiontermscondition_pk");

            entity.ToTable("TransactionTermsCondition");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Parameter1).HasColumnType("character varying");
            entity.Property(e => e.Parameter2).HasColumnType("character varying");
            entity.Property(e => e.ReferenceNumber).HasColumnType("character varying");
            entity.Property(e => e.Status).HasDefaultValueSql("1");
            entity.Property(e => e.TermsAndConditionCode).HasColumnType("character varying");
            entity.Property(e => e.TermsAndConditionDescription).HasColumnType("character varying");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "user_un").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, null, 2147483647L, null, null);
            entity.Property(e => e.BranchCode).HasColumnType("character varying");
            entity.Property(e => e.ContactNumber).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeactivateReason).HasMaxLength(255);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.Firstname).HasMaxLength(50);
            entity.Property(e => e.Fullname).HasColumnType("character varying");
            entity.Property(e => e.Fullname2).HasColumnType("character varying");
            entity.Property(e => e.HashCode).HasMaxLength(100);
            entity.Property(e => e.Lastname).HasMaxLength(50);
            entity.Property(e => e.Middlename).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Nickname).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PositionCode).HasColumnType("character varying");
            entity.Property(e => e.Sbu)
                .HasColumnType("character varying")
                .HasColumnName("SBU");
            entity.Property(e => e.Status).HasDefaultValueSql("1");
            entity.Property(e => e.Type).HasColumnType("character varying");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.VerifiedDate).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<UserBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userbranch_pk");

            entity.ToTable("UserBranch");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.BranchCode).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasDefaultValueSql("1");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.UserBranchCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("userbranch_fk_1");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.UserBranchModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .HasConstraintName("userbranch_fk_2");

            entity.HasOne(d => d.User).WithMany(p => p.UserBranchUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("userbranch_fk");
        });

        modelBuilder.Entity<UserMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usermenu_pk");

            entity.ToTable("UserMenu");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.BranchCode).HasColumnType("character varying");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasDefaultValueSql("1");

            entity.HasOne(d => d.User).WithMany(p => p.UserMenus)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("usermenu_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
