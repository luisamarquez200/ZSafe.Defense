using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Zombie> Zombies => Set<Zombie>();
        public DbSet<Simulation> Simulations => Set<Simulation>();
        public DbSet<Eliminated> Eliminated => Set<Eliminated>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Zombie>().ToTable("Zombies");
            modelBuilder.Entity<Simulation>().ToTable("Simulaciones");
            modelBuilder.Entity<Eliminated>().ToTable("Eliminados");

            modelBuilder.Entity<Eliminated>()
                .HasOne(e => e.Simulation)
                .WithMany(s => s.Eliminados)
                .HasForeignKey(e => e.SimulationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Eliminated>()
                .HasOne(e => e.Zombie)
                .WithMany()
                .HasForeignKey(e => e.ZombieId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}