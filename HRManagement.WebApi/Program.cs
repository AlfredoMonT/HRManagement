using HRManagement.Infrastructure.Persistence; // Tu DbContext
using Microsoft.EntityFrameworkCore;
using HRManagement.Application.Features.Employees.Commands.CreateEmployee; // Para que encuentre los comandos CQRS
using HRManagement.WebApi.Services; // Para tu servicio gRPC
using Microsoft.AspNetCore.Authentication.JwtBearer; // NUEVO: Para la seguridad
using Microsoft.IdentityModel.Tokens; // NUEVO: Para crear las llaves
using System.Text; // NUEVO: Para leer texto

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE SERVICIOS (CONTENEDOR)
// ==========================================

// Base de Datos (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<HrManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

// MediatR (Patrón CQRS - Requisito 6.4)
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly);
});

// gRPC (Requisito 6.3)
builder.Services.AddGrpc();

// Controladores y Swagger (Requisito 6.2)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- SEGURIDAD JWT (Requisito 6.6) ---
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

// Validación de seguridad por si olvidaste poner la clave en appsettings
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

// ==========================================
// 2. CONFIGURACIÓN DEL PIPELINE HTTP
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ¡IMPORTANTE! El orden aquí es crítico:
app.UseAuthentication(); // 1. Identificar al usuario (¿Quién eres?)
app.UseAuthorization();  // 2. Verificar permisos (¿Puedes pasar?)

app.MapControllers();

// Endpoint de gRPC
app.MapGrpcService<EmployeeGrpcService>();

app.Run();