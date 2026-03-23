using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlaneGameApi.Models;
using PlaneGameApi.Repositories;

namespace PlaneGameApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository _gameRepository;

    public GameController(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    [Authorize]
    [HttpPost("submit-score")]
    public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var username = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        await _gameRepository.SubmitScoreAsync(username, request.Score);
        return Ok(new { message = "Submit score successfully" });
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] int top = 10)
    {
        if (top <= 0)
        {
            return BadRequest(new { message = "top must be greater than 0" });
        }

        var leaderboard = await _gameRepository.GetTopScoresAsync(top);
        return Ok(leaderboard);
    }
}
