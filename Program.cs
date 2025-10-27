using Microsoft.EntityFrameworkCore;
using MyBffProject.Data;
using MyBffProject.Mapping;
using MyBffProject.Middleware;
using MyBffProject.Repositories;
using MyBffProject.Services;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Connection string: adicione em appsettings.json com chave "DefaultConnection"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)); // ou UseNpgsql, UseSqlite conforme seu BD

// DI
builder.Services.AddScoped<ITeamRepository, EfTeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// FluentValidation (registre seus validators)
builder.Services.AddControllers()
    .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Program>());


// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Outros: Auth, ResponseCaching, etc.
// builder.Services.AddAuthentication(...);
// builder.Services.AddAuthorization(...);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
