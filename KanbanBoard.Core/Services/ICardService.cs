using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoard.Core.DTOs;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Core.Services;

public interface ICardService
{
    Task<OperationResult> CreateCardAsync(string title, int columnId, int? assigneeId = null, string priority = "Medium");
    Task<OperationResult> MoveCardAsync(int cardId, int newColumnId);
    Task<OperationResult> DeleteCardAsync(int cardId);
    Task<List<Card>> GetCardsByColumnAsync(int columnId, CardFilter? filter = null);
}