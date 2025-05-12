using FootballRankingSystem.Data;
using FootballRankingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballRankingSystem.Services
{
	public class MatchService
	{
		private readonly AppDbContext _context;
		//public event MatchAddedEventHandler OnMatchAdd;
		//public event MatchUpdatedEventHandler OnMatchUpdated;
		//public event MatchDeletedEventHandler OnMatchDeleted;
		private readonly IEventBus _eventBus;

		public MatchService(AppDbContext context, IEventBus eventBus)
		{
			_context = context;
			_eventBus = eventBus;
		}

		public async Task<IEnumerable<MatchDTO>> GetAllMatchesAsync()
		{
			return await _context.Matches
				.Select(m => new MatchDTO
				{
					Id = m.Id,
					HomeTeamId = m.HomeTeamId,
					AwayTeamId = m.AwayTeamId,
					HomeScore = m.HomeScore,
					AwayScore = m.AwayScore,
					PlayedOn = m.PlayedOn
				})
				.ToListAsync();
		}

		public async Task<MatchDTO?> GetMatchByIdAsync(string id)
		{
			var match = await _context.Matches.FindAsync(id);
			if (match == null) return null;

			return new MatchDTO
			{
				Id = match.Id,
				HomeTeamId = match.HomeTeamId,
				AwayTeamId = match.AwayTeamId,
				HomeScore = match.HomeScore,
				AwayScore = match.AwayScore,
				PlayedOn = match.PlayedOn
			};
		}

		public async Task<MatchDTO> AddMatchAsync(MatchDTO matchDto)
		{
			var match = new Match
			{
				Id = matchDto.Id,
				HomeTeamId = matchDto.HomeTeamId,
				AwayTeamId = matchDto.AwayTeamId,
				HomeScore = matchDto.HomeScore,
				AwayScore = matchDto.AwayScore,
				PlayedOn = matchDto.PlayedOn
			};

			_context.Matches.Add(match);
			await _context.SaveChangesAsync();


			var @event = new MatchAddedEventArgs(match.HomeTeamId, match.AwayTeamId, match.HomeScore, match.AwayScore);
			_eventBus.Publish(this, @event);

			return matchDto;
		}

		public async Task<bool> UpdateMatchAsync(MatchDTO matchDto)
		{
			Match? match = await _context.Matches.FindAsync(matchDto.Id);

			int oldHomeScore = match.HomeScore;
			int oldAwayScore = match.AwayScore;

			if (match == null) return false;

			match.HomeTeamId = matchDto.HomeTeamId;
			match.AwayTeamId = matchDto.AwayTeamId;
			match.HomeScore = matchDto.HomeScore;
			match.AwayScore = matchDto.AwayScore;
			match.PlayedOn = matchDto.PlayedOn;

			_context.Entry(match).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			var @event = new MatchUpdatedEventArgs(matchDto.HomeTeamId, matchDto.AwayTeamId, oldHomeScore, matchDto.HomeScore, oldAwayScore, matchDto.AwayScore);
			_eventBus.Publish(this, @event);

			return true;
		}

		public async Task<bool> DeleteMatchAsync(string id)
		{

			
			var match = await _context.Matches.FindAsync(id);
			if (match == null)
			{
				throw new Exception("Match not found");

			}

			match.isDeleted = 1;
			_context.Entry(match).State = EntityState.Modified;

			await _context.SaveChangesAsync();
		
			var @event = new MatchDeletedEventArgs(match.HomeTeamId, match.AwayTeamId, match.HomeScore, match.AwayScore);
			_eventBus.Publish(this, @event);

			return true;
		}
	}
	public delegate Task MatchAddedEventHandler(string homeTeamId, string awayTeamId, int homeScore, int awayScore);
	
	public delegate Task MatchUpdatedEventHandler(string homeTeamId, string awayTeamId, int oldHomeScore, int newHomeScore, int oldAwayScore, int newAwayScore);
	
	public delegate Task MatchDeletedEventHandler(string homeTeamId, string awayTeamId, int homeScore, int awayScore);

}
