using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthorization();

app.MapGet("/", () => "Hello, World!");
app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
    .RequireAuthorization();
app.MapGet("/secret-claims", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret claims")
    .RequireAuthorization(p => p.RequireClaim("scope", "myapi:secrets"));
app.MapGet("/secret-manager", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret role")
    .RequireAuthorization(p => p.RequireRole("Manager"));

app.Run();