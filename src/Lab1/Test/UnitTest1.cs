using Lab1.Data;
using Lab1.Dtos;
using Lab1.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Lab1.Tests;

public class PersonServiceTests
{
    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistPersonWithId()
    {
        using var db = CreateDb();
        var service = new PersonService(db);

        var result = await service.CreateAsync(new PersonRequest
        {
            Name = "Иван",
            Age = 30,
            Address = "Москва",
            Work = "Разработчик"
        });

        Assert.True(result.Id > 0);
        Assert.Equal("Иван", result.Name);
        Assert.Equal(30, result.Age);
        Assert.Equal(1, await db.Persons.CountAsync());
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ReturnsNull()
    {
        using var db = CreateDb();
        var service = new PersonService(db);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPersons()
    {
        using var db = CreateDb();
        var service = new PersonService(db);
        await service.CreateAsync(new PersonRequest { Name = "A" });
        await service.CreateAsync(new PersonRequest { Name = "B" });

        var all = await service.GetAllAsync();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task UpdateAsync_ShouldChangeFields()
    {
        using var db = CreateDb();
        var service = new PersonService(db);
        var created = await service.CreateAsync(new PersonRequest { Name = "Old" });

        var updated = await service.UpdateAsync(created.Id, new PersonRequest
        {
            Name = "New",
            Age = 25
        });

        Assert.NotNull(updated);
        Assert.Equal("New", updated!.Name);
        Assert.Equal(25, updated.Age);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePerson()
    {
        using var db = CreateDb();
        var service = new PersonService(db);
        var created = await service.CreateAsync(new PersonRequest { Name = "ToDelete" });

        var deleted = await service.DeleteAsync(created.Id);
        var after = await service.GetByIdAsync(created.Id);

        Assert.True(deleted);
        Assert.Null(after);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ReturnsFalse()
    {
        using var db = CreateDb();
        var service = new PersonService(db);

        var result = await service.DeleteAsync(42);

        Assert.False(result);
    }
}