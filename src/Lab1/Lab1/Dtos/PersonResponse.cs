namespace Lab1.Dtos;

public class PersonResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Age { get; set; }
    public string? Address { get; set; }
    public string? Work { get; set; }
}