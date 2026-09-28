using Application.Exceptions;
using Domain.Entities;

namespace Application.Validators;
public class PropertyValidator
{
    public void ValidarPropiedad(Property propiedad)
    {
        // valida objeto null
        if (propiedad == null)
        {
            throw new Exception("La propiedad no puede ser nula.");
        }

        if (string.IsNullOrWhiteSpace(propiedad.Titulo))
        {
            throw new Exception("El título es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(propiedad.Ubicacion))
        {
            throw new Exception("La ubicación es obligatorio.");
        }
        
        if (propiedad.PrecioNoche <= 0)
        {
            throw new Exception("El precio debe ser mayor a 0.");
        }
        if (propiedad.Capacidad <= 0)
        {
            throw new Exception("La capacidad debe ser mayor a 0.");
        }
        if (propiedad.OwnerId <= 0)
        {
            throw new Exception("El propietario es obligatorio.");
        }

    }
}