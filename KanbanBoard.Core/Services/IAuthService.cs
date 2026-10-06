using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoard.Core.DTOs;

namespace KanbanBoard.Core.Services;

public interface IAuthService
{
    Task<OperationResult> RegisterAsync(string login, string password, string role = "Developer");
    Task<OperationResult> LoginAsync(string login, string password);
    Task<List<Models.User>> GetAllUsersAsync();
}