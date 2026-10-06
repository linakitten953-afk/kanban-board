namespace KanbanBoard.Api.DTOs;

public class OperationResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public static OperationResult SuccessResult() => new() { Success = true };
    public static OperationResult Fail(string msg) => new() { Success = false, ErrorMessage = msg };
}
