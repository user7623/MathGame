using MathGame.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MathGame.DataAccess
{
    public class MathGameDbContext : IdentityDbContext
    {
        public MathGameDbContext(DbContextOptions<MathGameDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GameRound>(en =>
            {
                en.HasKey(e => e.Id);
                en.ToTable("GameRound");
            });
        }

        public DbSet<GameRound> GameRounds { get; set; }
    }
}
