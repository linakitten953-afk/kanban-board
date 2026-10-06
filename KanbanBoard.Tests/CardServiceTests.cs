using KanbanBoard.Api.Data;
using KanbanBoard.Api.Models;
using KanbanBoard.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KanbanBoard.Tests;

public class CardServiceTests
{
    private AppDbContext GetContext()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(opts);
    }

    private async Task<Column> CreateTestColumnAsync(AppDbContext ctx, int? wipLimit = null)
    {
        var user = new User { Login = "u", PasswordHash = "h", Role = "Manager" };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var project = new Project { Name = "P", OwnerId = user.Id };
        ctx.Projects.Add(project);
        await ctx.SaveChangesAsync();

        var board = new Board { Name = "B", ProjectId = project.Id };
        ctx.Boards.Add(board);
        await ctx.SaveChangesAsync();

        var column = new Column { Name = "To Do", BoardId = board.Id, WipLimit = wipLimit };
        ctx.Columns.Add(column);
        await ctx.SaveChangesAsync();
        return column;
    }

    [Fact]
    public async Task CreateCard_ValidData_ReturnsSuccess()
    {
        var ctx = GetContext();
        var column = await CreateTestColumnAsync(ctx);
        var service = new CardService(ctx);

        var result = await service.CreateCardAsync("Задача 1", column.Id, null, "Medium");

        Assert.True(result.Success);
        Assert.Single(ctx.Cards);
    }

    [Fact]
    public async Task CreateCard_EmptyTitle_ReturnsFail()
    {
        var ctx = GetContext();
        var column = await CreateTestColumnAsync(ctx);
        var service = new CardService(ctx);

        var result = await service.CreateCardAsync("", column.Id, null, "Medium");

        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreateCard_WipLimitReached_ReturnsFail()
    {
        var ctx = GetContext();
        var column = await CreateTestColumnAsync(ctx, wipLimit: 1);
        var service = new CardService(ctx);
        await service.CreateCardAsync("1", column.Id, null, "Medium");

        var result = await service.CreateCardAsync("2", column.Id, null, "Medium");

        Assert.False(result.Success);
        Assert.Contains("WIP", result.ErrorMessage);
    }

    [Fact]
    public async Task MoveCard_ValidMove_UpdatesColumn()
    {
        var ctx = GetContext();
        var col1 = await CreateTestColumnAsync(ctx);
        var col2 = new Column { Name = "Done", BoardId = col1.BoardId };
        ctx.Columns.Add(col2);
        await ctx.SaveChangesAsync();

        var service = new CardService(ctx);
        await service.CreateCardAsync("Task", col1.Id, null, "Medium");
        var cardId = ctx.Cards.First().Id;

        var result = await service.MoveCardAsync(cardId, col2.Id);

        Assert.True(result.Success);
        Assert.Equal(col2.Id, ctx.Cards.Find(cardId)!.ColumnId);
    }

    [Fact]
    public async Task DeleteCard_Existing_RemovesFromDb()
    {
        var ctx = GetContext();
        var column = await CreateTestColumnAsync(ctx);
        var service = new CardService(ctx);
        await service.CreateCardAsync("Task", column.Id, null, "Medium");
        var cardId = ctx.Cards.First().Id;

        var result = await service.DeleteCardAsync(cardId);

        Assert.True(result.Success);
        Assert.Empty(ctx.Cards);
    }

    [Fact]
    public async Task GetCards_FilterByPriority_ReturnsCorrect()
    {
        var ctx = GetContext();
        var column = await CreateTestColumnAsync(ctx);
        var service = new CardService(ctx);
        await service.CreateCardAsync("Low card", column.Id, null, "Low");
        await service.CreateCardAsync("High card", column.Id, null, "High");

        var result = await service.GetCardsByColumnAsync(column.Id,
            new KanbanBoard.Api.DTOs.CardFilter { Priority = "High" });

        Assert.Single(result);
        Assert.Equal("High", result[0].Priority);
    }
}