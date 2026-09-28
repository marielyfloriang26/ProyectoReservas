using Application.Services;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.ExternalServices;
using Infrastructure.Repositories; // Ajusta según carpetas
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURACIÓN DE JWT (Esto arregla el error 500 del parámetro 's')
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["SecretKey"] ?? "ProyectoReservas_MarielyFlorian_2026_SecretKey"))
        };
    });

    builder.Services.AddAuthorization();

// Esto le dice a la API que busque los Controllers
builder.Services.AddControllers()
    .AddJsonOptions(x => 
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro de dependencias (Inyección de dependencias)
// CONFIGURACIÓN DE LA BASE DE DATOS
// Reemplaza 'AppDbContext' por el nombre real de tu clase de base de datos
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// REGISTRO DE REPOSITORIOS (Capa Infrastructure)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IBlockedDateRepository, BlockedDateRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
// Esto le dice a la app: "Cuando el UserService pida IPasswordHasher, dale esta clase"
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();



// REGISTRO DE SERVICIOS (Capa Application)
// contienen lógica de negocio
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PropertyService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<AvailabilityService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReviewService>();

// Si tienes un servicio de Email, regístralo también
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseCors();
app.UseMiddleware<API.Middleware.ExceptionMiddleware>();
// Activar Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} // Esto crea la página visual

app.UseHttpsRedirection();

// Agregar estos dos para que el candado de Swagger funcione
app.UseAuthentication(); 
app.UseAuthorization();

// Esto mapea las rutas de los controladores automáticamente
app.MapControllers();

app.Run();