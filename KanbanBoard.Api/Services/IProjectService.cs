using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Models;

namespace KanbanBoard.Api.Services;

public interface IProjectService
{
    Task<OperationResult> CreateAsync(string name, int ownerId);
    Task<List<Project>> GetUserProjectsAsync(int userId);
    Task<Project?> GetByIdAsync(int id);
    Task<OperationResult> DeleteAsync(int id, int userId, string role);
}