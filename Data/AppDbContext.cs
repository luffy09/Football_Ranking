using FootballRankingSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace FootballRankingSystem.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options) { }

		// DbSets = tables
		public DbSet<Team> Teams { get; set; }
		public DbSet<Match> Matches { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Team>()
				.Property(t => t.TotalScore)
				.HasDefaultValue(0);

			//Relation between match and team(Home)
			modelBuilder.Entity<Match>()
				.HasOne(m => m.HomeTeam)
				.WithMany()
				.HasForeignKey(m => m.HomeTeamId)
				.OnDelete(DeleteBehavior.Restrict);

			//Relation between match and team(Away)
			modelBuilder.Entity<Match>()
				.HasOne(m => m.AwayTeam)
				.WithMany()
				.HasForeignKey(m => m.AwayTeamId)
				.OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(modelBuilder);
		}
	}
}