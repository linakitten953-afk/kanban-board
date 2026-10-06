using KanbanBoard.Api.Data;
using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    public ProjectService(AppDbContext context) => _context = context;

    public async Task<OperationResult> CreateAsync(string name, int ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult.Fail("Название проекта обязательно");

        _context.Projects.Add(new Project { Name = name, OwnerId = ownerId });
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<List<Project>> GetUserProjectsAsync(int userId) =>
        await _context.Projects
            .Where(p => p.OwnerId == userId)
            .Include(p => p.Boards)
            .ToListAsync();

    public async Task<Project?> GetByIdAsync(int id) =>
        await _context.Projects
            .Include(p => p.Boards)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<OperationResult> DeleteAsync(int id, int userId, string role)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return OperationResult.Fail("Проект не найден");

        if (role != "Admin" && project.OwnerId != userId)
            return OperationResult.Fail("Нет прав на удаление");

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }
}