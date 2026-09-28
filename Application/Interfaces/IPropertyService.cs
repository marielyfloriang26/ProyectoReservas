using Domain.Entities;
using Application.DTOs;

namespace Application.Interfaces;

public interface IPropertyService
{
    Task CrearPropiedad(PropertyDto dto, int ownerId);
    Task<PropertyDto> ActualizarPropiedad(PropertyDto dto, int ownerId);
    Task EliminarPropiedad(int id, int ownerId);
    Task<PropertyDto?> ObtenerPropiedad(int id);
    
    // El PDF pide filtros específicos
    Task<List<PropertyDto>> BuscarPropiedades(PropertyDto filtros);
}