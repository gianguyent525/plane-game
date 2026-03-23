using PlaneGameApi.Models;

namespace PlaneGameApi.Repositories;

public interface IAuthRepository
{
    Task RegisterUserAsync(string username, string passwordHash);
    Task<UserLoginInfoDto?> GetUserForLoginAsync(string username);
}
