using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoard.Core.Data;
using KanbanBoard.Core.DTOs;
using KanbanBoard.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Core.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher _hasher;

    public AuthService(AppDbContext context, PasswordHasher hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public async Task<OperationResult> RegisterAsync(string login, string password, string role = "Developer")
    {
        if (string.IsNullOrWhiteSpace(login))
            return OperationResult.Fail("Логин обязателен");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            return OperationResult.Fail("Пароль минимум 4 символа");

        if (await _context.Users.AnyAsync(u => u.Login == login))
            return OperationResult.Fail("Пользователь уже существует");

        var user = new Models.User
        {
            Login = login,
            PasswordHash = _hasher.Hash(password),
            Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<OperationResult> LoginAsync(string login, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null || !_hasher.Verify(password, user.PasswordHash))
            return OperationResult.Fail("Неверный логин или пароль");
        return OperationResult.SuccessResult();
    }

    public async Task<List<Models.User>> GetAllUsersAsync() =>
        await _context.Users.ToListAsync();
}