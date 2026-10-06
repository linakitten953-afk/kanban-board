using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoard.Core.DTOs;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Core.Services;

public interface IProjectService
{
    Task<OperationResult> CreateAsync(string name, int ownerId);
    Task<List<Project>> GetUserProjectsAsync(int userId);
}