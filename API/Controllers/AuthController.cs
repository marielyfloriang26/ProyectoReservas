using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Persistence;
using Infrastructure.ExternalServices;
using Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    private readonly IEmailService _emailService;

    private readonly IJwtProvider _jwtProvider;
    public AuthController(ApplicationDBContext context, IEmailService emailService, IJwtProvider jwtProvider)
    {
        _context = context;
        _emailService = emailService;
        _jwtProvider = jwtProvider;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
{
    // Validar si existe
    if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
    {
        return BadRequest("Este correo electrónico ya está registrado.");
    }

    // Generamos el código de 6 dígitos
    string codigoRandom = new Random().Next(100000, 999999).ToString();

    // Crear el usuario
    var nuevoUsuario = new User
    {
        Nombre = registerDto.Nombre,
        Email = registerDto.Email,
        PasswordHash = registerDto.Password, // encriptar luego con BCrypt
        Confirmada = false,
        ConfirmacionToken = codigoRandom,
        ExpiracionToken = DateTime.Now.AddHours(24)
    };

    _context.Users.Add(nuevoUsuario);
    await _context.SaveChangesAsync(); // Aquí se guarda en la DB

    // Llamar al servicio de correo 
    try 
    {
      // El "!" después de ConfirmacionToken le quita la advertencia al compilador
        await _emailService.EnviarCorreoConfirmacionAsync(nuevoUsuario.Email, nuevoUsuario.Nombre, nuevoUsuario.ConfirmacionToken);
    }
    catch (Exception ex)
    {
        // Esto imprimirá el error en tu terminal si Mailpit falla
        Console.WriteLine($"Ocurrió un error al intentar enviar: {ex.Message}");
    }

    return Ok(new { message = "¡Registro exitoso! Revisa tu correo.", userId = nuevoUsuario.Id });
}

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Buscar al usuario por email y password
        var usuario = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

        if (usuario == null)
        {
            // Retornamos 401 si las credenciales no coinciden
            return Unauthorized("Email o contraseña incorrectos.");
        }
      
    // Usamos el jwtProvider que inyectamos en el constructor para crear la "llave"
   var tokenGerenerado = _jwtProvider.Generate(usuario);

        // Si todo está bien, retornamos los datos básicos para que el Front-end los use
        return Ok(new { 
            message = "Bienvenido al sistema", 
          token = tokenGerenerado,
            userId = usuario.Id, 
            nombre = usuario.Nombre 
        });
    }
        
    [HttpPost("confirmar")]
    public async Task<IActionResult> Confirmar([FromBody] ConfirmarDto confirmarDto)
{
    // Buscamos al usuario que coincida con el email y el código
    var usuario = await _context.Users.FirstOrDefaultAsync(u => 
        u.Email == confirmarDto.Email && u.ConfirmacionToken == confirmarDto.Codigo);

    if (usuario == null)
    {
        return BadRequest("El código es incorrecto o el correo no existe.");
    }

    // Si lo encuentra, activamos la cuenta
    usuario.Confirmada = true;
    usuario.ConfirmacionToken = null; // Limpiamos el código para que no se use de nuevo
    
    await _context.SaveChangesAsync();

    return Ok(new { message = "Cuenta confirmada con éxito. ¡Ya puedes iniciar sesión!" });
}
    }

