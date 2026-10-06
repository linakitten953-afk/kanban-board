using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Models;

namespace KanbanBoard.Api.Services;

public interface IBoardService
{
    Task<OperationResult> CreateBoardAsync(string name, int projectId);
    Task<OperationResult> CreateColumnAsync(string name, int boardId, int? wipLimit);
    Task<List<Board>> GetBoardsAsync(int projectId);
    Task<List<Column>> GetColumnsAsync(int boardId);
}