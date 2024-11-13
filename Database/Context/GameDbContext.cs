using EngineeredAngel.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeredAngel.Database.Context
{
    public class GameDbContext : DbContext
    {

        public DbSet<GamePlayerEntity> Player { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EngineeredAngel;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
