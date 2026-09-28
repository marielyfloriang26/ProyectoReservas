namespace Application.Exceptions;
public class NotFoundException : Exception
{
    // constructor simple
    public NotFoundException(string message) : base(message)
    {
    }
    // 2do constructor 
    // Constructor avanzado que ahorra escribir el mensaje cada vez
    // Ejemplo de uso: throw new NotFoundException("Propiedad", 15);
    public NotFoundException(string entity, object key) 
        : base($"El registro '{entity}' con identificador ({key}) no fue encontrado.") { }
}