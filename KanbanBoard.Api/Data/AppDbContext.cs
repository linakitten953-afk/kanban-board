using KanbanBoard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<Column> Columns => Set<Column>();
    public DbSet<Card> Cards => Set<Card>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Card>()
          .HasOne(c => c.Column)
          .WithMany(col => col.Cards)
          .HasForeignKey(c => c.ColumnId)
          .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Card>()
          .HasOne(c => c.Assignee)
          .WithMany()
          .HasForeignKey(c => c.AssigneeId)
          .OnDelete(DeleteBehavior.SetNull);

        mb.Entity<Project>()
          .HasOne(p => p.Owner)
          .WithMany()
          .HasForeignKey(p => p.OwnerId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}