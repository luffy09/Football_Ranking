using FootballRankingSystem.Data;
using Microsoft.EntityFrameworkCore;

public class TeamService
{
	private readonly AppDbContext _context;
	private readonly IServiceScopeFactory _scopeFactory;


	public TeamService(AppDbContext context, IEventBus eventBus, IServiceScopeFactory scopeFactory)
	{
		_context = context;
		eventBus.Subscribe<MatchAddedEventArgs>(OnMatchAdded);
		eventBus.Subscribe<MatchUpdatedEventArgs>(OnMatchUpdated);
		eventBus.Subscribe<MatchDeletedEventArgs>(OnMatchDeleted);
		_scopeFactory = scopeFactory;
	}


	public async Task<bool> IsTeamChanged(Team team)
	{
		var t = await _context.Teams.FindAsync(team.Id);

		if (t == null) {
			throw new Exception("Team not found");
		}

		return t.Equals(team);
	}

	public async Task<TeamDTO> GetTeamByIdAsync(string id)
	{
		var team = await _context.Teams.Where(t => t.isDeleted == 0 && t.Id == id)
				  .Select(t => new TeamDTO
				  {
					  Id = t.Id,
					  Name = t.Name,
					  TotalScore = t.TotalScore,
					  CreatedOn = t.CreatedOn,
					  isActive = t.isActive == 1,
				  })
				.FirstOrDefaultAsync();


		return team ?? throw new Exception("Team not found");
	}

	public async Task<List<TeamDTO>> GetAllTeamsAsync()
	{
		// As you mentioned in the interview(thx for the hint btw) we first use IQueryable to filter/order our data	
		IQueryable<TeamDTO> orderedTeams = _context.Teams
			.Where(t => t.isDeleted == 0)
			.OrderByDescending(t => t.TotalScore)   // First, order by TotalScore (highest first)
			.ThenByDescending(t => t.CreatedOn)     // Then, order by CreatedOn (newest first)
													// Yes the DTO is almost pointless but it hides the isDeleted Property 
			.Select(t => new TeamDTO
			{
				Id = t.Id,
				Name = t.Name,
				TotalScore = t.TotalScore,
				CreatedOn = t.CreatedOn,
				isActive = t.isActive == 1,
			});



		// And then we conver it to IEnumerable
		return await orderedTeams.ToListAsync();
	}

	public async Task<TeamDTO> AddTeam(TeamDTO teamDTO)
	{
		teamDTO.Id = Guid.NewGuid().ToString();

		Team team = new Team( teamDTO.Id, teamDTO.Name, teamDTO.TotalScore, DateTime.Now, 1, 0);


		_context.Teams.Add(team);
		await _context.SaveChangesAsync();


	

		return teamDTO;
	}

	public async Task UpdateTeamAsync(TeamDTO teamDTO)
	{
		var existingTeam = await _context.Teams.FindAsync(teamDTO.Id);
		if (existingTeam == null)
		{
			throw new Exception("Team not found");

		}

		Team team = new Team(teamDTO.Id, teamDTO.Name, teamDTO.TotalScore, teamDTO.CreatedOn, teamDTO.isActive ? 1 : 0, 0);



		// Usually I would defend this on the front end, but this won't have a front end
		// if nothing is actually changed there is no need to bother the database with pointless requests
		if (!existingTeam.Equals(team))
		{
			existingTeam.Name = teamDTO.Name;
			existingTeam.TotalScore = teamDTO.TotalScore;
			existingTeam.CreatedOn = teamDTO.CreatedOn;
			existingTeam.isActive = teamDTO.isActive ? 1 : 0;


			_context.Entry(existingTeam).State = EntityState.Modified;

			await _context.SaveChangesAsync();
		}
	}

	public async Task DeleteTeamAsync(string Id)
	{
		var team = await _context.Teams.FindAsync(Id);

		if (team == null)
		{
			throw new Exception("Team not found");

		}

		team.isDeleted = 1;
		_context.Entry(team).State = EntityState.Modified;

		await _context.SaveChangesAsync();
	}

	public void OnMatchAdded(MatchAddedEventArgs args)
	{
		using (var scope = _scopeFactory.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var homeTeam = context.Teams.Find(args.HomeTeamId);
			var awayTeam = context.Teams.Find(args.AwayTeamId);

			if (homeTeam == null || awayTeam == null)
			{
				throw new ArgumentException("One or both teams not found");
			}

			if (args.NewHomeScore > args.NewAwayScore) homeTeam.TotalScore += 3;
			else if (args.NewAwayScore > args.NewHomeScore) awayTeam.TotalScore += 3;
			else
			{
				homeTeam.TotalScore += 1;
				awayTeam.TotalScore += 1;
			}

			context.Entry(homeTeam).State = EntityState.Modified;
			context.Entry(awayTeam).State = EntityState.Modified;

			context.SaveChanges();
		}
	}

	public void OnMatchUpdated(MatchUpdatedEventArgs args)
	{
		using (var scope = _scopeFactory.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var homeTeam = context.Teams.Find(args.HomeTeamId);
			var awayTeam = context.Teams.Find(args.AwayTeamId);

			if (homeTeam == null || awayTeam == null)
			{
				throw new ArgumentException("One or both teams not found");
			}

			if (args.OldHomeScore > args.OldAwayScore) homeTeam.TotalScore -= 3;
			else if (args.OldAwayScore > args.OldHomeScore) awayTeam.TotalScore -= 3;
			else { homeTeam.TotalScore -= 1; awayTeam.TotalScore -= 1; }


			if (args.NewHomeScore > args.NewAwayScore) homeTeam.TotalScore += 3;
			else if (args.NewAwayScore > args.NewHomeScore) awayTeam.TotalScore += 3;
			else { homeTeam.TotalScore += 1; awayTeam.TotalScore += 1; }


			context.Entry(homeTeam).State = EntityState.Modified;
			context.Entry(awayTeam).State = EntityState.Modified;

			context.SaveChanges();
		}
	}

	
	public void OnMatchDeleted(MatchDeletedEventArgs args)
	{
		using (var scope = _scopeFactory.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var homeTeam = context.Teams.Find(args.HomeTeamId);
			var awayTeam = context.Teams.Find(args.AwayTeamId);

			if (homeTeam == null || awayTeam == null)
			{
				throw new ArgumentException("One or both teams not found");
			}


			if (args.NewHomeScore > args.NewAwayScore) homeTeam.TotalScore -= 3;
			else if (args.NewAwayScore > args.NewHomeScore) awayTeam.TotalScore -= 3;
			else { homeTeam.TotalScore -= 1; awayTeam.TotalScore -= 1; }

			// Save the updated team scores to the database
			context.Entry(homeTeam).State = EntityState.Modified;
			context.Entry(awayTeam).State = EntityState.Modified;

			context.SaveChanges();
		}
	}
}