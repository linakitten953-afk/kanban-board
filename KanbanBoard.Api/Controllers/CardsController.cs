using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CardsController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardsController(ICardService cardService)
    {
        _cardService = cardService;
    }

    /// <summary>
    /// Карточки в колонке с фильтрацией.
    /// </summary>
    [HttpGet("column/{columnId}")]
    public async Task<IActionResult> GetByColumn(int columnId,
        [FromQuery] string? search, [FromQuery] string? priority, [FromQuery] int? assigneeId)
    {
        var filter = new CardFilter
        {
            SearchText = search,
            Priority = priority,
            AssigneeId = assigneeId
        };

        var cards = await _cardService.GetCardsByColumnAsync(columnId, filter);
        return Ok(cards);
    }

    /// <summary>
    /// Создать карточку.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCardDto dto)
    {
        var result = await _cardService.CreateCardAsync(
            dto.Title, dto.ColumnId, dto.AssigneeId, dto.Priority);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Переместить карточку в другую колонку.
    /// </summary>
    [HttpPut("{id}/move/{newColumnId}")]
    public async Task<IActionResult> Move(int id, int newColumnId)
    {
        var result = await _cardService.MoveCardAsync(id, newColumnId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Удалить карточку.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _cardService.DeleteCardAsync(id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

public class CreateCardDto
{
    public string Title { get; set; } = string.Empty;
    public int ColumnId { get; set; }
    public int? AssigneeId { get; set; }
    public string Priority { get; set; } = "Medium";
}