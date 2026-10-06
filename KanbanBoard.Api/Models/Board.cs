using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KanbanBoard.Api.Models;

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    public List<Column> Columns { get; set; } = new();
}