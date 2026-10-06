using KanbanBoard.Api.Data;
using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Services;

public class CardService : ICardService
{
    private readonly AppDbContext _context;
    public CardService(AppDbContext context) => _context = context;

    public async Task<OperationResult> CreateCardAsync(string title, int columnId,
        int? assigneeId, string priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            return OperationResult.Fail("Название обязательно");

        var column = await _context.Columns
            .Include(c => c.Cards)
            .FirstOrDefaultAsync(c => c.Id == columnId);

        if (column == null) return OperationResult.Fail("Колонка не найдена");

        if (column.WipLimit.HasValue && column.Cards.Count >= column.WipLimit.Value)
            return OperationResult.Fail($"WIP-лимит колонки '{column.Name}' достигнут");

        _context.Cards.Add(new Card
        {
            Title = title,
            ColumnId = columnId,
            AssigneeId = assigneeId,
            Priority = priority
        });
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<OperationResult> MoveCardAsync(int cardId, int newColumnId)
    {
        var card = await _context.Cards.FindAsync(cardId);
        if (card == null) return OperationResult.Fail("Карточка не найдена");

        var newColumn = await _context.Columns
            .Include(c => c.Cards)
            .FirstOrDefaultAsync(c => c.Id == newColumnId);

        if (newColumn == null) return OperationResult.Fail("Колонка не найдена");

        if (newColumn.WipLimit.HasValue && newColumn.Cards.Count >= newColumn.WipLimit.Value)
            return OperationResult.Fail("WIP-лимит превышен");

        card.ColumnId = newColumnId;
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<OperationResult> DeleteCardAsync(int cardId)
    {
        var card = await _context.Cards.FindAsync(cardId);
        if (card == null) return OperationResult.Fail("Карточка не найдена");

        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<List<Card>> GetCardsByColumnAsync(int columnId, CardFilter? filter)
    {
        var query = _context.Cards
            .Include(c => c.Assignee)
            .Where(c => c.ColumnId == columnId);

        if (!string.IsNullOrWhiteSpace(filter?.SearchText))
            query = query.Where(c => c.Title.Contains(filter.SearchText));

        if (!string.IsNullOrWhiteSpace(filter?.Priority))
            query = query.Where(c => c.Priority == filter.Priority);

        if (filter?.AssigneeId.HasValue == true)
            query = query.Where(c => c.AssigneeId == filter.AssigneeId);

        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }
}