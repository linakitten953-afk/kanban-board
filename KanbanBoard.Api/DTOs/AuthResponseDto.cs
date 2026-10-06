namespace KanbanBoard.Api.DTOs;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Login { get; set; }
    public string? Role { get; set; }
}