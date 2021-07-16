using Microsoft.EntityFrameworkCore;

#nullable disable

namespace API.Repository
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Payment> Payments { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CardHolderName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Cvv).HasColumnName("CVV");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrderReference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PaymentId).HasDefaultValueSql("(newid())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
