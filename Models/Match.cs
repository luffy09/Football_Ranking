using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballRankingSystem.Models
{
	public class Match
	{
		// ID is better as GUID but because this will be used in swagger(where we need to work with the ids) I am keeping it as string to be humanly readable
		[Key]
		public  string Id { get; set; }


		[ForeignKey("HomeTeam")]
		public string HomeTeamId { get; set; }
		public  Team HomeTeam { get; set; }

		[ForeignKey("AwayTeam")]
		public  string AwayTeamId { get; set; }
		public Team AwayTeam { get; set; }

		public int HomeScore { get; set; }
		public int AwayScore { get; set; }

		public DateTime PlayedOn { get; set; }

		// It can be added(Created) to the statistics later than it was played
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public int isActive { get; set; } = 1;
		public int isDeleted { get; set; } = 0;
		public Match() { }


		public Match(string homeTeamId, string awayTeamId, int homeScore, int awayScore, DateTime playedOn)
		{
			Id = Guid.NewGuid().ToString(); // Automatically generate Id
			HomeTeamId = homeTeamId;
			AwayTeamId = awayTeamId;
			HomeScore = homeScore;
			AwayScore = awayScore;
			PlayedOn = playedOn;
			CreatedOn = DateTime.Now; // Default value for CreatedOn
			isActive = 1; // Default value for isActive
			isDeleted = 0; // Default value for isDeleted
		}
		public Match(string id, string homeTeamId, string awayTeamId, int homeScore, int awayScore, DateTime playedOn, DateTime createdOn, int isActive, int isDeleted)
		{
			Id = id; // Allow setting the Id, in case you want to manually provide it.
			HomeTeamId = homeTeamId;
			AwayTeamId = awayTeamId;
			HomeScore = homeScore;
			AwayScore = awayScore;
			PlayedOn = playedOn;
			CreatedOn = createdOn;
			this.isActive = isActive;
			this.isDeleted = isDeleted;
		}


	}
}