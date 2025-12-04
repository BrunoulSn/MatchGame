using BFF_GameMatch.Services;
using BFF_GameMatch.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// backend continua em 5182
var backendUrl = builder.Configuration["Backend:BaseUrl"] ?? "http://localhost:5182";
Console.WriteLine($"🔗 Configurando conexão com backend: {backendUrl}");

builder.Services.AddHttpClient<IUserService, UserService>(c =>
{
    c.BaseAddress = new Uri(backendUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<IGroupService, GroupService>(c =>
{
    c.BaseAddress = new Uri(backendUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddAuthentication("DevAuth")
    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions,
        BFF_GameMatch.Authentication.DevAuthHandler>("DevAuth", null);
builder.Services.AddHttpClient("GameMatchApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5182"); // Porta do seu back GameMatch.Api
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowFrontend", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("✅ BFF rodando (porta configurada via launchSettings.json)");
Console.WriteLine($"🔗 Backend configurado em {backendUrl}");

app.Run();
