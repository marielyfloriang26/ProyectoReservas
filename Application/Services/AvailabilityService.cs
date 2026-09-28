using System;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class AvailabilityService
{
    // variables privadas, dependencias
    private readonly IReservationRepository _reservationRepository;
    private readonly IBlockedDateRepository _blockedDateRepository;

    // constructor, inyeccion de dependencias
    public AvailabilityService(IReservationRepository _reservationRepository,IBlockedDateRepository _blockedDateRepository)
    {
        this._reservationRepository = _reservationRepository; // asigna parametro a varible
        this._blockedDateRepository = _blockedDateRepository;
    }

    public async Task<bool> estaDisponible(int propertyId, DateTime fechaInicio, DateTime fechaFin) 
    {
       // Obtener todas las reservas de esa propiedad
       var reservasExistentes = await _reservationRepository.GetPorPropiedad(propertyId);

       // Verificar si alguna reserva choca con las fechas solicitadas
       bool tieneChoqueReservas = reservasExistentes.Any(r => fechaInicio < r.FechaFin && fechaFin > r.FechaInicio);

       if (tieneChoqueReservas)
        {
            return false;
        }

        // Verificar si hay fechas bloqueadas por el Host (mantenimiento, uso personal, etc.)
        var fechasBloqueadas = await _blockedDateRepository.ObtenerPorPropiedad(propertyId);

        bool tieneChoqueBloqueos = fechasBloqueadas.Any(b => fechaInicio < b.FechaFin && fechaFin > b.FechaInicio);

        if (tieneChoqueBloqueos)
        {
            return false;
        }

        // si paso ambos filtros, la propiedad esta libre
        return true;
    }
    
}