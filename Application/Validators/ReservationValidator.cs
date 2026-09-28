using Application.Exceptions;
using Domain.Entities;

namespace Application.Validators;
public class ReservationValidator
{
    public void ValidarReserva(Reservation reservacion)
    {
        // validad objeto null
        if (reservacion == null)
        {
            throw new Exception("La reservación no puede ser nula.");
        }
        if (reservacion.FechaInicio >= reservacion.FechaFin)
        {
            throw new InvalidStateException("La fecha de inicio debe ser menor que la fecha de fin.");
        }
        if (reservacion.FechaInicio < DateTime.Now)
        {
            throw new InvalidStateException("La fecha de inicio no puede estar en el pasado.");
        }
        // validar ids
        if (reservacion.PropertyId <= 0)
        {
            throw new Exception("La propiedad es obligatoria.");
        }
        if (reservacion.UserId <= 0)
        {
            throw new Exception("El usuario es obligatorio.");
        }
    }
}