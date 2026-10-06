using KanbanBoard.Api.DTOs;

namespace KanbanBoard.Api.Services;

public interface IAuthService
{
    Task<OperationResult> RegisterAsync(string login, string password, string role);
    Task<AuthResponseDto> LoginAsync(string login, string password);
    Task<List<Models.User>> GetAllUsersAsync();
}