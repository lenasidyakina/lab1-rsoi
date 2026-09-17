using Lab1.Data;
using Lab1.Dtos;
using Lab1.Interfaces;
using Lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Services;

public class PersonService : IPersonService
{
    private readonly AppDbContext _db;

    public PersonService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<PersonResponse>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Persons
            .AsNoTracking()
            .Select(p => ToResponse(p))
            .ToListAsync(ct);
    }

    public async Task<PersonResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var person = await _db.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        return person is null ? null : ToResponse(person);
    }

    public async Task<PersonResponse> CreateAsync(PersonRequest request, CancellationToken ct = default)
    {
        var person = new Person
        {
            Name = request.Name.Trim(),
            Age = request.Age,
            Address = request.Address,
            Work = request.Work
        };

        _db.Persons.Add(person);
        await _db.SaveChangesAsync(ct);

        return ToResponse(person);
    }

    public async Task<PersonResponse?> UpdateAsync(int id, PersonRequest request, CancellationToken ct = default)
    {
        var person = await _db.Persons.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (person is null) return null;

        person.Name = request.Name.Trim();
        if (request.Age     is not null) person.Age     = request.Age;
        if (request.Address is not null) person.Address = request.Address;
        if (request.Work    is not null) person.Work    = request.Work;

        await _db.SaveChangesAsync(ct);

        return ToResponse(person);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var person = await _db.Persons.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (person is null) return false;

        _db.Persons.Remove(person);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private static PersonResponse ToResponse(Person p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Age = p.Age,
        Address = p.Address,
        Work = p.Work
    };
}