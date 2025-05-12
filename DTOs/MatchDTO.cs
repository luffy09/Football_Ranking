public class MatchDTO
{
	public string Id { get; set; }
	public string HomeTeamId { get; set; }
	public string AwayTeamId { get; set; }
	public int HomeScore { get; set; }
	public int AwayScore { get; set; }
	public DateTime PlayedOn { get; set; }
}