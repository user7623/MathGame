using MathGame.Models;
using Microsoft.EntityFrameworkCore;


namespace MathGame.DataAccess
{
    public class MathGameDbContext : DbContext
    {
        public MathGameDbContext(DbContextOptions<MathGameDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameRound>(en =>
            {
                en.HasKey(e => e.Id);
                en.ToTable("GameRound");
            });
        }

        public DbSet<GameRound> GameRounds { get; set; }
    }
}
