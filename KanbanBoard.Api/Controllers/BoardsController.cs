using KanbanBoard.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    /// <summary>
    /// Список досок в проекте.
    /// </summary>
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProject(int projectId)
    {
        var boards = await _boardService.GetBoardsAsync(projectId);
        return Ok(boards);
    }

    /// <summary>
    /// Создать доску.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoardDto dto)
    {
        var result = await _boardService.CreateBoardAsync(dto.Name, dto.ProjectId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Список колонок доски.
    /// </summary>
    [HttpGet("{boardId}/columns")]
    public async Task<IActionResult> GetColumns(int boardId)
    {
        var columns = await _boardService.GetColumnsAsync(boardId);
        return Ok(columns);
    }

    /// <summary>
    /// Создать колонку.
    /// </summary>
    [HttpPost("columns")]
    public async Task<IActionResult> CreateColumn([FromBody] CreateColumnDto dto)
    {
        var result = await _boardService.CreateColumnAsync(dto.Name, dto.BoardId, dto.WipLimit);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

public class CreateBoardDto
{
    public string Name { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}

public class CreateColumnDto
{
    public string Name { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public int? WipLimit { get; set; }
}