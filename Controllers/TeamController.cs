using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballRankingSystem.Data;
using FootballRankingSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using FootballRankingSystem.Services;

namespace FootballRankingSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TeamController : ControllerBase
	{
		private readonly TeamService _teamService;

		public TeamController( TeamService teamService)
		{
			_teamService = teamService;

		}
	
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
		{
			return  Ok(await _teamService.GetAllTeamsAsync()); 
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TeamDTO>> GetTeam(string id)
		{
			var team = await _teamService.GetTeamByIdAsync(id);
			return team == null ? NotFound() : Ok(team);
		}


		[HttpPost]
		public async Task<ActionResult<TeamDTO>> AddTeam(TeamDTO team)
		{
			team.Id = Guid.NewGuid().ToString();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			TeamDTO teamDTO = await _teamService.AddTeam(team);
		

			return CreatedAtAction("GetTeam", new { id = teamDTO.Id }, teamDTO);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditTeam(string id, TeamDTO team)
		{
			if (id != team.Id)
			{
				// Can't have editing one team with another
				// Another option is to directly take just one team and use the id from inside of it
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _teamService.UpdateTeamAsync(team);

			return Ok();
		}


		[HttpDelete("{id}")]

		public async Task<IActionResult> DeleteTeam(string id)
		{
			await _teamService.DeleteTeamAsync(id);

			return Ok();
		}
	}
}
