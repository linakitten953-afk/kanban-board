using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoard.Core.Data;
using KanbanBoard.Core.DTOs;
using KanbanBoard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Core.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    public ProjectService(AppDbContext context) => _context = context;

    public async Task<OperationResult> CreateAsync(string name, int ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult.Fail("Название проекта обязательно");

        var project = new Project { Name = name, OwnerId = ownerId };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<List<Project>> GetUserProjectsAsync(int userId) =>
        await _context.Projects
            .Where(p => p.OwnerId == userId)
            .Include(p => p.Boards)
            .ToListAsync();
}