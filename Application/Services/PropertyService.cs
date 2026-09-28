using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class PropertyService
{
    // variables privadas, dependencias
    private readonly IPropertyRepository _propertyRepository;

    private readonly IReservationRepository _reservationRepository;

    // constructor, inyeccion de dependencias
    public PropertyService(IPropertyRepository _propertyRepository, IReservationRepository _reservationRepository)
    {
        this._propertyRepository = _propertyRepository; // asigna parametro a varible
        this._reservationRepository = _reservationRepository;
    }

    public async Task<Property> CrearPropiedad(Property propiedad) // cuando el host publica
    {
       // validaciones
       if(propiedad.PrecioNoche <= 0)
        {
            throw new Exception("El precio por noche debe ser mayor a cero.");
        }
        if (string.IsNullOrEmpty(propiedad.Titulo))
        {
            throw new Exception("El nombre de la propiedad es obligatorio.");
        }

        // guardar usando el repositorio
        await _propertyRepository.CrearPropiedad(propiedad); 

        return propiedad;
    }
    public async Task ActualizarPropiedad(Property propiedad)
    {
        // validar que exista antes de intentar actualizar
        var existe = await _propertyRepository.GetPorId(propiedad.Id);
        
        if(existe == null)
        {
            throw new Exception("No se puede actualizar una propiedad que no existe.");
        }
        await _propertyRepository.ActualizarPropiedad(propiedad);
    }
    
    public async Task EliminarPropiedad(int propertyId)
    {
        // verificar si existe 
        var propiedad = await _propertyRepository.GetPorId(propertyId);
        if(propiedad == null)
        {
            throw new Exception("La propiedad que intenta eliminar no existe.");
        }
        // Verificar si tiene reservas antes de borrar
    var tieneReservas = await _reservationRepository.GetPorPropiedad(propertyId);

    if (tieneReservas.Any()) 
    {
        throw new Exception("No puedes eliminar una propiedad que tiene reservas.");
    }

    // eliminar
    await _propertyRepository.EliminarPropiedad(propertyId);
    }

    public async Task<Property?> ObtenerPropiedadPorId(int propertyId)
    {
        var propiedad = await _propertyRepository.GetPorId(propertyId);
        if(propiedad == null)
        {
            throw new Exception("La propiedad solicitada no existe.");
        }
        return propiedad;
    }
     public async Task <List<Property>> ObtenerTodas()
    {
        return await _propertyRepository.GetTodas();
    }
    
}