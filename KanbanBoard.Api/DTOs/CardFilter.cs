namespace KanbanBoard.Api.DTOs;

public class CardFilter
{
    public string? SearchText { get; set; }
    public string? Priority { get; set; }
    public int? AssigneeId { get; set; }
}