using Domain.Entities;
using Application.DTOs;


namespace Application.Interfaces;

public interface IUserService
{
    Task RegistrarUsuario(RegisterUserDto dto);
    
    // Devuelve un DTO que contenga el string del Token JWT

    // ojo crear authresponsedto en dtos en vez de string
    Task<string> Login(LoginDto dto);

    Task ConfirmarCuenta(string token);
}