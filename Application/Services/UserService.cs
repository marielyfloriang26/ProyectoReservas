using Domain.Entities;
using Domain.Interfaces;
using System.Threading.Tasks;
using Application.Interfaces; // Para EmailService

namespace Application.Services;

public class UserService
{
    // variables privadas, dependencias
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHash;
    // constructor, inyeccion de dependencias
    private readonly IJwtProvider _jwtProvider;
    private readonly IEmailService _emailService;

    // Inyectamos todas las herramientas necesarias
    public UserService(IUserRepository _userRepository, IPasswordHasher passwordHash, IJwtProvider jwtProvider, IEmailService emailService)
    {
        this._userRepository = _userRepository; // asigna parametro a varible
        this._passwordHash = passwordHash;
        this._jwtProvider = _jwtProvider;
        this._emailService = _emailService;
    }

    public async Task<User> RegistrarUsuario(User usuario)
    {
       // verificar si el email ya existe en la BD
       var existe = await _userRepository.GetPorEmail(usuario.Email);
       if (existe != null)
        {
            throw new Exception("El correo electrónico ya está registrado.");
        }
        // hashear la contra antes de guardar
        usuario.PasswordHash = _passwordHash.Hash(usuario.PasswordHash);

        // preparar token de confirmacion (se puede usar un guid o un codigo aleatorio)
        usuario.ConfirmacionToken = Guid.NewGuid().ToString().Substring(0, 8);
        usuario.Confirmada = false;

        // guardar en la base de datos mediante el repositorio 
        await _userRepository.GuardarUsuario(usuario);
        

        // enviar el correo de confirmacion 
        await _emailService.EnviarCorreoConfirmacionAsync(usuario.Email, usuario.Nombre, usuario.ConfirmacionToken);
        return usuario;
    }

    public async Task<string> LoginUsuario(string Email, string Password)
    {
        // buscar el usuario
        var usuario = await _userRepository.GetPorEmail(Email);

        // validar existencia y contrasena 
        if (usuario == null || !_passwordHash.Verify(Password, usuario.PasswordHash))
        {
            throw new Exception("Credenciales incorrectas.");
        }

        // validar si la cuenta esta confirmada 
        if (!usuario.Confirmada)
        {
            throw new Exception("Debes confirmar tu cuenta antes de iniciar sesión. ");
        }

        // generar y devolver el token jwt
        return _jwtProvider.Generate(usuario); // generate

    }
    
    public async Task ConfirmarCuenta(string ConfirmacionToken)
    {
        // buscar al usuario por el token o buscar de forma general y filtrar

        // EJEMPLO:
        var usuario = await _userRepository.GetPorEmail(ConfirmacionToken);

        if (usuario != null && usuario.ConfirmacionToken == ConfirmacionToken)
        {
            usuario.Confirmada = true;
            usuario.ConfirmacionToken = null; //limpiar token

            await _userRepository.ActualizarUsuario(usuario);
        }
        else
        {
            
            throw new Exception("Token de confirmación inválido.");
        }
    }
    public async Task<List<User>> ObtenerTodos() 
    {
    return await _userRepository.ObtenerTodos(); // Revisa que el repo se llame así
    }

    public async Task<User?> ObtenerPorId(int id) 
    {
    return await _userRepository.GetPorId(id);
    }
    public async Task ActualizarUsuario(User usuario) 
    {
    await _userRepository.ActualizarUsuario(usuario);
    }
    public async Task<User?> ObtenerPorEmail(string email)
    {
    // Le pedimos al repositorio que busque al usuario que coincida con ese correo
    return await _userRepository.GetPorEmail(email);
    }
    
}