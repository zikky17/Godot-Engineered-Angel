using EngineeredAngel.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeredAngel.Database.Context
{
    public class GameDbContext : DbContext
    {

        public DbSet<GamePlayerEntity> Player { get; set; }
        public DbSet<PlayerInventoryEntity> Inventory { get; set; }
        public DbSet<LootItemEntity> LootItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EngineeredAngel;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerInventoryEntity>()
                .HasMany(i => i.LootItems)
                .WithOne()
                .HasForeignKey(li => li.InventoryId);
        }
    }
}
