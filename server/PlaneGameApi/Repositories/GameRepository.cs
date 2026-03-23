using Dapper;
using Microsoft.Data.SqlClient;
using PlaneGameApi.Models;

namespace PlaneGameApi.Repositories;

public class GameRepository : IGameRepository
{
    private readonly string _connectionString;

    public GameRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing in appsettings.json");
    }

    public async Task SubmitScoreAsync(string username, int score)
    {
        await using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@username", username);
        parameters.Add("@score", score);

        await connection.ExecuteAsync(
            "dbo.sp_submit_score",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<LeaderboardEntryDto>> GetTopScoresAsync(int top)
    {
        await using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@top", top);

        return await connection.QueryAsync<LeaderboardEntryDto>(
            "dbo.sp_get_top_scores",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
