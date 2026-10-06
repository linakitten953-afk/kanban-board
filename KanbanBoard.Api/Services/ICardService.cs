using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Models;

namespace KanbanBoard.Api.Services;

public interface ICardService
{
    Task<OperationResult> CreateCardAsync(string title, int columnId, int? assigneeId, string priority);
    Task<OperationResult> MoveCardAsync(int cardId, int newColumnId);
    Task<OperationResult> DeleteCardAsync(int cardId);
    Task<List<Card>> GetCardsByColumnAsync(int columnId, CardFilter? filter);
}