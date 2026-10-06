using KanbanBoard.Api.Data;
using KanbanBoard.Api.Helpers;
using KanbanBoard.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace KanbanBoard.Tests;

public class AuthServiceTests
{
    private AppDbContext GetContext()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(opts);
    }

    private ITokenService GetTokenService()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "TEST_KEY_FOR_UNIT_TESTS_1234567890ABCDEF",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            })
            .Build();
        return new TokenService(config);
    }

    [Fact]
    public async Task Register_NewUser_ReturnsSuccess()
    {
        var ctx = GetContext();
        var service = new AuthService(ctx, new PasswordHasher(), GetTokenService());

        var result = await service.RegisterAsync("testuser", "password123", "Developer");

        Assert.True(result.Success);
        Assert.Single(ctx.Users);
    }

    [Fact]
    public async Task Register_DuplicateLogin_ReturnsFail()
    {
        var ctx = GetContext();
        var service = new AuthService(ctx, new PasswordHasher(), GetTokenService());
        await service.RegisterAsync("testuser", "password123", "Developer");

        var result = await service.RegisterAsync("testuser", "other", "Developer");

        Assert.False(result.Success);
        Assert.Contains("уже существует", result.ErrorMessage);
    }

    [Fact]
    public async Task Register_EmptyLogin_ReturnsFail()
    {
        var ctx = GetContext();
        var service = new AuthService(ctx, new PasswordHasher(), GetTokenService());

        var result = await service.RegisterAsync("", "password123", "Developer");

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Register_ShortPassword_ReturnsFail()
    {
        var ctx = GetContext();
        var service = new AuthService(ctx, new PasswordHasher(), GetTokenService());

        var result = await service.RegisterAsync("user1", "12", "Developer");

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Login_CorrectCredentials_ReturnsToken()
    {
        var ctx = GetContext();
        var service = new AuthService(ctx, new PasswordHasher(), GetTokenService());
        await service.RegisterAsync("user1", "password123", "Developer");

        var result = await service.LoginAsync("user1", "password123");

        Assert.True(result.Success);
        Assert.NotNull(result.Token);
        Assert.Equal("user1", result.Login);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsFail()
    {
        var ctx = GetContext();
        var service = new AuthService(ctx, new PasswordHasher(), GetTokenService());
        await service.RegisterAsync("user1", "password123", "Developer");

        var result = await service.LoginAsync("user1", "wrong");

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Login_NonExistentUser_ReturnsFail()
    {
        var ctx = GetContext();
        var service = new AuthService(ctx, new PasswordHasher(), GetTokenService());

        var result = await service.LoginAsync("nobody", "password123");

        Assert.False(result.Success);
    }

    [Fact]
    public async Task PasswordHasher_HashAndVerify_Works()
    {
        var hasher = new PasswordHasher();
        var hash = hasher.Hash("secret");

        Assert.True(hasher.Verify("secret", hash));
        Assert.False(hasher.Verify("wrong", hash));
    }
}