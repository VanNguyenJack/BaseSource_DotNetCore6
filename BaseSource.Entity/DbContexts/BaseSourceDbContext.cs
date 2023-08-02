using BaseSource.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BaseSource.Domain.Catalog;
namespace BaseSource.Entity.DbContexts;

public partial class BaseSourceDbContext : DbContext, IApplicationDbContext
{
    public BaseSourceDbContext(DbContextOptions<BaseSourceDbContext> options)
        : base(options)
    {
    }

    public IDbConnection Connection => Database.GetDbConnection();

    public bool HasChanges => ChangeTracker.HasChanges();

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1P1MAVM;Database=BaseSourceDB;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Uid);

            entity.ToTable("Account");

            entity.Property(e => e.CostCenter).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(500);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Uid);

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Insurance).HasMaxLength(100);
            entity.Property(e => e.IsCanRunInsideLsp).HasColumnName("IsCanRunInsideLSP");
            entity.Property(e => e.LimitKmtravelled).HasColumnName("LimitKMTravelled");
            entity.Property(e => e.TotalKmtravelled).HasColumnName("TotalKMTravelled");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.VehiclePlateNumber).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
