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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCardDto dto)
    {
        var result = await _cardService.CreateCardAsync(
            dto.Title, dto.ColumnId, dto.AssigneeId, dto.Priority, dto.Deadline);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCardDto dto)
    {
        var result = await _cardService.UpdateCardAsync(
            id, dto.Title, dto.Priority, dto.Deadline, dto.AssigneeId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}/move/{newColumnId}")]
    public async Task<IActionResult> Move(int id, int newColumnId)
    {
        var result = await _cardService.MoveCardAsync(id, newColumnId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

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
    public DateTime? Deadline { get; set; }
}

public class UpdateCardDto
{
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public DateTime? Deadline { get; set; }
    public int? AssigneeId { get; set; }
}