using System.Security.Claims;
using KanbanBoard.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private string CurrentUserRole =>
        User.FindFirstValue(ClaimTypes.Role) ?? "Developer";

    /// <summary>
    /// Список проектов текущего пользователя.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetUserProjectsAsync(CurrentUserId);
        return Ok(projects);
    }

    /// <summary>
    /// Создать проект.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var result = await _projectService.CreateAsync(dto.Name, CurrentUserId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Удалить проект. Доступно только владельцу или Admin.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteAsync(id, CurrentUserId, CurrentUserRole);
        if (!result.Success) return Forbid(result.ErrorMessage ?? "Нет прав");
        return Ok(result);
    }
}

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
}