using System.ComponentModel.DataAnnotations;

namespace Lab1.Dtos;

public class PersonRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 150)]
    public int? Age { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(200)]
    public string? Work { get; set; }
}