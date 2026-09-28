using Domain.Entities;

namespace Domain.Interfaces;
public interface IUserRepository
{
    Task GuardarUsuario(User usuario); // Task para que sea async
    Task<User> GetPorId(int id);
    Task<User> GetPorEmail(string email);
    Task<List<User>> ObtenerTodos();
    Task ActualizarUsuario(User usuario);
}