using KanbanBoard.Api.Data;
using KanbanBoard.Api.DTOs;
using KanbanBoard.Api.Helpers;
using KanbanBoard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext context, PasswordHasher hasher, ITokenService tokenService)
    {
        _context = context;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<OperationResult> RegisterAsync(string login, string password, string role)
    {
        if (string.IsNullOrWhiteSpace(login))
            return OperationResult.Fail("Логин обязателен");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            return OperationResult.Fail("Пароль минимум 4 символа");

        if (await _context.Users.AnyAsync(u => u.Login == login))
            return OperationResult.Fail("Пользователь уже существует");

        var user = new User
        {
            Login = login,
            PasswordHash = _hasher.Hash(password),
            Role = role
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return OperationResult.SuccessResult();
    }

    public async Task<AuthResponseDto> LoginAsync(string login, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null || !_hasher.Verify(password, user.PasswordHash))
            return new AuthResponseDto { Success = false, ErrorMessage = "Неверный логин или пароль" };

        var token = _tokenService.GenerateToken(user);
        return new AuthResponseDto
        {
            Success = true,
            Token = token,
            Login = user.Login,
            Role = user.Role
        };
    }

    public async Task<List<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();
}