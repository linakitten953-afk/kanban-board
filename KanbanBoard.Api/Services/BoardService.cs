using KanbanBoard.Api.Data;
using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Services;

public class BoardService : IBoardService
{
    private readonly AppDbContext _context;
    public BoardService(AppDbContext context) => _context = context;

    public async Task<OperationResult> CreateBoardAsync(string name, int projectId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult.Fail("Название доски обязательно");

        _context.Boards.Add(new Board { Name = name, ProjectId = projectId });
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<OperationResult> CreateColumnAsync(string name, int boardId, int? wipLimit)
    {
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult.Fail("Название колонки обязательно");

        var order = await _context.Columns.CountAsync(c => c.BoardId == boardId);
        _context.Columns.Add(new Column
        {
            Name = name,
            BoardId = boardId,
            Order = order,
            WipLimit = wipLimit
        });
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<List<Board>> GetBoardsAsync(int projectId) =>
        await _context.Boards.Where(b => b.ProjectId == projectId).ToListAsync();

    public async Task<List<Column>> GetColumnsAsync(int boardId) =>
        await _context.Columns
            .Where(c => c.BoardId == boardId)
            .Include(c => c.Cards)
            .OrderBy(c => c.Order)
            .ToListAsync();
}