using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using MyBffProject.Data;
using MyBffProject.Mapping;
using MyBffProject.Middleware;
using MyBffProject.Repositories;
using MyBffProject.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using MyBffProject.Authentication;
using Microsoft.Extensions.Logging;
using BFF_GameMatch.Services;

var builder = WebApplication.CreateBuilder(args);

// Serilog - lê configuração do appsettings (instale Serilog.Settings.Configuration)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// registra Serilog como logger do host (instale Serilog.AspNetCore)
builder.Host.UseSerilog();

// Connection string: adicione em appsettings.json com chave "DefaultConnection"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString)); // ou UseNpgsql, UseSqlite conforme seu BD
}
else
{
    // fallback para InMemory para desenvolvimento quando não houver connection string
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("MatchGame_Dev"));
}

// DI
builder.Services.AddScoped<ITeamRepository, EfTeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IUserService, UserService>();

// HttpClient para backend
var backendUrl = builder.Configuration["Backend:BaseUrl"] ?? "http://localhost:5000";
builder.Services.AddHttpClient("backend", client => client.BaseAddress = new Uri(backendUrl));

// Register backend proxy service and httpcontext accessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBackendProxyService, BackendProxyService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// FluentValidation + Controllers
builder.Services.AddControllers()
    .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Program>());

// Health checks
builder.Services.AddHealthChecks();

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000", "http://localhost:5173" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        if (allowedOrigins.Length == 1 && allowedOrigins[0] == "*")
        {
            policy.AllowAnyOrigin();
        }
        else
        {
            policy.WithOrigins(allowedOrigins);
        }
        policy.AllowAnyHeader().AllowAnyMethod();
    });
});

// Simple development authentication (accepts header X-User-Id or Authorization: Bearer {userId})
builder.Services.AddAuthentication("Dev")
    .AddScheme<AuthenticationSchemeOptions, DevAuthHandler>("Dev", options => { });
builder.Services.AddAuthorization();

// swagger apenas em dev
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}
// Configure pipeline
var app = builder.Build();

// Em Development, exibir página de exceção detalhada para testes e debugging
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Em produção, registrar middleware global de tratamento de exceções
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseRouting();
app.UseCors("DefaultCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/ready");
app.MapHealthChecks("/health/live");

app.Run();
//dotnet add package Serilog.AspNetCore -	dotnet add package Serilog.Settings.Configuration -	dotnet add package Serilog.Sinks.Console -	dotnet restore
//dotnet build