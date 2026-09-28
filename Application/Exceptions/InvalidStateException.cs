namespace Application.Exceptions;
public class InvalidStateException : Exception
{
    public InvalidStateException(string message) : base(message)
    {
    }
    //2do constructor, util para cuando la transición de estado no es permitida
    public InvalidStateException(string estadoActual, string estadoDestino) 
        : base($"No es posible cambiar el estado de '{estadoActual}' a '{estadoDestino}'.") { }
}