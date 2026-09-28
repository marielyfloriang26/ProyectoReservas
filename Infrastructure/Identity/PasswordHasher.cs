using Application.Interfaces;

namespace Infrastructure.Identity;

public class PasswordHasher : IPasswordHasher
{
    // Antes de generar el token, necesito asegurarme de que la contraseña sea segura. Esta clase se encarga de "hashear" (encriptar) la clave antes de guardarla en la BD.

    // convierte la contra de texto plano a un hash seguro
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

        // verifica si la contra escrita coincide con el hash de la base de datos
    public bool Verify(string password, string PasswordHash) =>
        BCrypt.Net.BCrypt.Verify(password, PasswordHash);
}