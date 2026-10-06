using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoard.Core.DTOs;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Core.Services;

public interface IBoardService
{
    Task<OperationResult> CreateBoardAsync(string name, int projectId);
    Task<OperationResult> CreateColumnAsync(string name, int boardId, int? wipLimit = null);
    Task<List<Board>> GetBoardsAsync(int projectId);
    Task<List<Column>> GetColumnsAsync(int boardId);
}