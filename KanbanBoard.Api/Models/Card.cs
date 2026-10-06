namespace KanbanBoard.Api.Models;

public class Card
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = "Medium";
    public DateTime? Deadline { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int ColumnId { get; set; }
    public Column? Column { get; set; }
    public int? AssigneeId { get; set; }
    public User? Assignee { get; set; }
}