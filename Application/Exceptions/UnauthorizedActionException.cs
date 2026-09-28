namespace Application.Exceptions;
public class UnauthorizedActionException : Exception
{
    // constructor simple
    public UnauthorizedActionException(string message) : base(message)
    {
    }
    // Mensaje por defecto para ahorrar código
    public UnauthorizedActionException() 
        : base("No tienes los permisos necesarios para realizar esta acción sobre este recurso.") { }
}