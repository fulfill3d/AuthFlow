using Microsoft.EntityFrameworkCore;

namespace AuthFlow.Data.Database
{
    public class AuthFlowDbContext(DbContextOptions<AuthFlowDbContext> options) : DbContext(options)
    {
        public DbSet<Entity> Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                
                entity.Property(e => e.RefId)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                
                entity.Property(e => e.UpdatedAt)
                    .IsRequired();
                
                entity.Property(e => e.IsEnabled)
                    .IsRequired();
                
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);
            });
        }
    }
}