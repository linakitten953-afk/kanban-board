using KanbanBoard.Api.Models;

namespace KanbanBoard.Api.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}