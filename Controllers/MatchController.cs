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
	public class MatchController : ControllerBase
	{
		private readonly MatchService _matchService;


		public MatchController(MatchService matchService)
		{
			_matchService = matchService;


		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MatchDTO>>> GetMatches()
		{
			var matches = await _matchService.GetAllMatchesAsync();
			return Ok(matches);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<MatchDTO>> GetMatch(string id)
		{
			var match = await _matchService.GetMatchByIdAsync(id);
			return match == null ? NotFound() : Ok(match);
		}

		[HttpPost]
		public async Task<ActionResult<MatchDTO>> AddMatch(MatchDTO matchDto)
		{
			matchDto.Id = Guid.NewGuid().ToString();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			MatchDTO addedMatch = await _matchService.AddMatchAsync(matchDto);
			return CreatedAtAction(nameof(GetMatch), new { id = addedMatch.Id }, addedMatch);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditMatch(string id, MatchDTO matchDto)
		{
			if (id != matchDto.Id)
			{
				return BadRequest("ID mismatch");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _matchService.UpdateMatchAsync(matchDto);

			if (!result)
				return NotFound();

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMatch(string id)
		{
			var deleted = await _matchService.DeleteMatchAsync(id);

			if (!deleted)
				return NotFound();

			return Ok();
		}
	}
}
