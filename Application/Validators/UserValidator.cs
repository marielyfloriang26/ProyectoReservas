using Application.Exceptions;
using Domain.Entities;

namespace Application.Validators;
public class UserValidator
{
    public void ValidarUsuario(User usuario)
    {
        // valida objeto null
        if (usuario == null)
        {
            throw new Exception("El usuario no puede ser nulo.");
        }
        if (string.IsNullOrWhiteSpace(usuario.Nombre))
        {
            throw new Exception("El nombre es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(usuario.Email))
        {
            throw new Exception("El email es obligatorio.");
        }
        // Email valido
        if (!usuario.Email.Contains("@"))
        {
            throw new Exception("El email no es válido. Debe contener @");
        }
        if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
        {
            throw new Exception("La contraseña es obligatoria.");
        }
        // Longitud mínima
        if (usuario.PasswordHash.Length < 6)
        {
            throw new Exception("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}