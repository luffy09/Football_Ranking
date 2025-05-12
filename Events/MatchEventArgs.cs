using FootballRankingSystem.Models;

public abstract class MatchEventArgs : EventArgs
{
	public string HomeTeamId { get; }
	public string AwayTeamId { get; }
	public int NewHomeScore { get; }
	public int NewAwayScore { get; }

	protected MatchEventArgs(string homeTeamId, string awayTeamId, int newHomeScore, int newAwayScore)
	{
		HomeTeamId = homeTeamId;
		AwayTeamId = awayTeamId;
	}
}

public class MatchAddedEventArgs : MatchEventArgs
{
	public MatchAddedEventArgs(string homeTeamId, string awayTeamId, int homeScore, int awayScore)
		: base(homeTeamId, awayTeamId, homeScore, awayScore)
	{
	}
}
public class MatchDeletedEventArgs : MatchEventArgs
{
	public MatchDeletedEventArgs(string homeTeamId, string awayTeamId, int homeScore, int awayScore)
		: base(homeTeamId, awayTeamId, homeScore, awayScore)
	{
	}
}

public class MatchUpdatedEventArgs : MatchEventArgs
{
	public int OldHomeScore { get; }
	public int OldAwayScore { get; }

	public MatchUpdatedEventArgs(
		string homeTeamId,
		string awayTeamId,
		int oldHomeScore,
		int newHomeScore,
		int oldAwayScore,
		int newAwayScore)
		: base(homeTeamId, awayTeamId, newHomeScore, newAwayScore)
	{
		OldHomeScore = oldHomeScore;
		OldAwayScore = oldAwayScore;
	}
}