using System;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class ReviewService
{
    // variables privadas, dependencias
    private readonly IReservationRepository _reservationRepository;
    private readonly IReviewRepository _reviewRepository;

    // constructor, inyeccion de dependencias
    public ReviewService(IReservationRepository _reservationRepository,IReviewRepository _reviewRepository)
    {
        this._reservationRepository = _reservationRepository; // asigna parametro a varible
        this._reviewRepository = _reviewRepository;
    }

    public async Task<Review> CrearReview(Review resena) 
    {
        // logica seria guardar en repository y luego devolver resena
        // Validar si existe la reserva a la que se le quiere hacer la reseña
        var reserva = await _reservationRepository.GetPorId(resena.ReservationId);

        if (reserva == null)
        {
            throw new Exception("No se puede comentar una reserva que no existe.");
        }

        if(reserva.EstadoReserva != ReservationStatus.Completada)
        {
            throw new Exception("Solo puedes dejar una reseña si tu estancia ya ha finalizado y la reserva está completada.");
        }

        // (Opcional) Validar que no exista ya una reseña para esta reserva (para evitar duplicados)

        var existeResena = await _reviewRepository.ObtenerPorReservaId(resena.ReservationId);

        if (existeResena != null) 
        {
            throw new Exception("Ya has calificado esta estancia.");
        }
        
        // guardar resena
        await _reviewRepository.CrearResena(resena);

        return resena;
    }
    
    // Método adicional para obtener reseñas de una propiedad específica
    public async Task<List<Review>> ObtenerResenasPorPropiedad(int propertyId)
    {
        return await _reviewRepository.ObtenerPorPropiedad(propertyId);
    }
}