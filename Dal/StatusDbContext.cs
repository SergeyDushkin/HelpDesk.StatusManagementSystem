using Microsoft.EntityFrameworkCore;
using servicedesk.StatusManagementSystem.Domain;

namespace servicedesk.StatusManagementSystem.Dal
{
    public class StatusDbContext : DbContext
    {
        public StatusDbContext(DbContextOptions<StatusDbContext> options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusEvent>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<StatusEvent>().ToTable("WH_STATUS_EVENTS");
            modelBuilder.Entity<StatusEvent>().HasKey(r => r.Id);
            modelBuilder.Entity<StatusEvent>().Ignore(r => r.CreatedAt);
            modelBuilder.Entity<StatusEvent>().Ignore(r => r.UpdatedAt);

            modelBuilder.Entity<StatusSource>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<StatusSource>().ToTable("WH_STATUS_SOURCES");
            modelBuilder.Entity<StatusSource>().HasKey(r => r.Id);
            modelBuilder.Entity<StatusSource>().Ignore(r => r.CreatedAt);
            modelBuilder.Entity<StatusSource>().Ignore(r => r.UpdatedAt);

            modelBuilder.Entity<Status>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Status>().ToTable("WH_STATUSES");
            modelBuilder.Entity<Status>().HasKey(r => r.Id);
            modelBuilder.Entity<Status>().Ignore(r => r.CreatedAt);
            modelBuilder.Entity<Status>().Ignore(r => r.UpdatedAt);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<StatusEvent> StatusEvents { get; set; }
        public DbSet<StatusSource> StatusSources { get; set; }
        public DbSet<Status> Statuses { get; set; }
    }
}
