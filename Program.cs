using Microsoft.OpenApi.Models;
using BFF_GameMatch.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BFF GameMatch", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Bearer {seu_token_jwt}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme{ Reference=new OpenApiReference{ Type=ReferenceType.SecurityScheme, Id="Bearer"}}, Array.Empty<string>() }
    });
});

builder.Services.AddCors(opt =>
{
    var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ??
                  new[] { "http://localhost:5173", "http://localhost:3000" };
    opt.AddDefaultPolicy(p => p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

builder.Services.AddResponseCaching();
builder.Services.AddBackendHttpClient(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseResponseCaching();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
