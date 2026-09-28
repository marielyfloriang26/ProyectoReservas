using Domain.Entities;

namespace Domain.Interfaces;
public interface IPropertyRepository
{
    Task CrearPropiedad(Property propiedad);
    Task<Property?> GetPorId(int id);
    Task<List<Property>> GetTodas();
    Task ActualizarPropiedad(Property propiedad);
    Task EliminarPropiedad(int id);
}