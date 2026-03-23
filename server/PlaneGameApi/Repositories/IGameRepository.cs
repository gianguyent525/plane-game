using PlaneGameApi.Models;

namespace PlaneGameApi.Repositories;

public interface IGameRepository
{
    Task SubmitScoreAsync(string username, int score);
    Task<IEnumerable<LeaderboardEntryDto>> GetTopScoresAsync(int top);
}
