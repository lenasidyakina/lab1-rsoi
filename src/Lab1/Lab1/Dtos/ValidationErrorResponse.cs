namespace Lab1.Dtos;

public class ValidationErrorResponse
{
    public string Message { get; set; } = "Validation failed";
    public Dictionary<string, string> Errors { get; set; } = new();
}
