using Lab1.Data;
using Lab1.Dtos;
using Lab1.Interfaces;
using Lab1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var rawConn = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")!;
var connString = NormalizePostgresUrl(rawConn);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connString));
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(kv => kv.Value?.Errors.Count > 0)
            .ToDictionary(
                kv => kv.Key,
                kv => kv.Value!.Errors.First().ErrorMessage);

        return new BadRequestObjectResult(new ValidationErrorResponse
        {
            Message = "Validation failed",
            Errors = errors
        });
    };
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.Run();

static string NormalizePostgresUrl(string url)
{
    if (!url.StartsWith("postgres://") && !url.StartsWith("postgresql://"))
        return url;

    var uri = new Uri(url);
    var userInfo = uri.UserInfo.Split(':', 2);
    
    var isLocal = uri.Host is "localhost" or "127.0.0.1" or "host.docker.internal";
    var sslPart = isLocal
        ? ""
        : "SSL Mode=Require;Trust Server Certificate=true;";

    return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};" +
           $"Username={userInfo[0]};Password={userInfo[1]};{sslPart}";
}

public partial class Program { }