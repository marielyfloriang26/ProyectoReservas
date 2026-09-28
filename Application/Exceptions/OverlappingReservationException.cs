namespace Application.Exceptions;
public class OverlappingReservationException : Exception
{
    public OverlappingReservationException(string message) : base(message)
    {
    }
    // 2do constructor
    public OverlappingReservationException() 
        : base("Las fechas seleccionadas no están disponibles o se solapan con una reserva existente.") { }
}