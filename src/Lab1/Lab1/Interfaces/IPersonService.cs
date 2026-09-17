using Lab1.Dtos;

namespace Lab1.Interfaces;

public interface IPersonService
{
    Task<IReadOnlyList<PersonResponse>> GetAllAsync(CancellationToken ct = default);
    Task<PersonResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PersonResponse> CreateAsync(PersonRequest request, CancellationToken ct = default);
    Task<PersonResponse?> UpdateAsync(int id, PersonRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}