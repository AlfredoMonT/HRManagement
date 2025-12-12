using HRManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using HRManagement.Application.Features.Employees.Commands.CreateEmployee;
using HRManagement.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HRManagement.Application.Interfaces;
using Microsoft.OpenApi.Models; 

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS

// Base de Datos (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<HrManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

// Conectar la Interfaz con la Base de Datos Real
builder.Services.AddScoped<IHrManagementDbContext>(provider => provider.GetRequiredService<HrManagementDbContext>());

// MediatR (Patrón CQRS)
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly);
});

// gRPC 
builder.Services.AddGrpc();

// Controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRManagement API", Version = "v1" });

    // Definimos el esquema de seguridad (Bearer)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
// -----------------------------------------------------

// --- SEGURIDAD JWT ---
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("¡ERROR! La clave 'Jwt:Key' no está configurada en appsettings.json");
}

var keyBytes = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

builder.Services.AddAuthorization();
// -------------------------------------

var app = builder.Build();

// 2. CONFIGURACIÓN DEL PIPELINE HTTP


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // 1. Identificar al usuario
app.UseAuthorization();  // 2. Verificar permisos

app.MapControllers();
app.MapGrpcService<EmployeeGrpcService>();

app.Run();