using Dapper;
using Microsoft.Data.SqlClient;
using PlaneGameApi.Models;

namespace PlaneGameApi.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    public AuthRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing in appsettings.json");
    }

    public async Task RegisterUserAsync(string username, string passwordHash)
    {
        await using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@username", username);
        parameters.Add("@password_hash", passwordHash);

        await connection.ExecuteAsync(
            "dbo.sp_register_user",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<UserLoginInfoDto?> GetUserForLoginAsync(string username)
    {
        await using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@username", username);

        return await connection.QuerySingleOrDefaultAsync<UserLoginInfoDto>(
            "dbo.sp_get_user_for_login",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
