using Domain.Entities;

namespace Domain.Interfaces;

public interface IBlockedDateRepository
{
    Task CrearFechaBloqueada(BlockedDate fecha);
    Task<List<BlockedDate>> ObtenerPorPropiedad(int propertyId);
    Task EliminarFechaBloqueada(int id); // Útil si el host quiere liberar la fecha
}