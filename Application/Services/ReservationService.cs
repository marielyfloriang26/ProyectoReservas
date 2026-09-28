using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class ReservationService
{
    // variables privadas, dependencias
    private readonly IReservationRepository _reservationRepository; // Para guardar la reserva.
    private readonly IPropertyRepository _propertyRepository; //Para saber cuánto cuesta la noche y quién es el dueño.
    private readonly AvailabilityService _availabilityService;
    private readonly IUserRepository _userRepository; // Necesario para cargar datos del Guest

    private readonly IEmailService _emailService;

    // constructor, inyeccion de dependencias
    public ReservationService(IReservationRepository _reservationRepository, IPropertyRepository _propertyRepository, AvailabilityService _availabilityService, IEmailService _emailService, IUserRepository userRepository)
    {
        this._reservationRepository = _reservationRepository; // asigna parametro a varible

        this._propertyRepository = _propertyRepository;
        this._availabilityService = _availabilityService;
         this._emailService = _emailService;
         this._userRepository = userRepository;
    }

    public async Task<Reservation> CrearReserva(Reservation reserva)
    {
        // validar fechas basicas
        if(reserva.FechaInicio >= reserva.FechaFin)
        {
            throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin.");
        }
        if(reserva.FechaInicio < DateTime.Now)
        {
            throw new Exception("No puedes realizar una reserva para una fecha pasada.");
        }

        // verificar disponibilidad usando el servicio especializado
        bool disponibilidad = await _availabilityService.estaDisponible(
            reserva.PropertyId,
            reserva.FechaInicio,
            reserva.FechaFin
        );


        if (!disponibilidad)
        {
            throw new Exception("La propiedad no está disponible para las fechas seleccionadas.");
        }

        // obt propiedad con su owner
        var propiedad = await _propertyRepository.GetPorId(reserva.PropertyId);
        if(propiedad == null)
        {
            throw new Exception("La propiedad seleccionada no existe.");
        }

        // calcular precio total automaticamente
        int noches = (reserva.FechaFin - reserva.FechaInicio).Days;
        reserva.PrecioTotal = noches * propiedad.PrecioNoche;

        // configurar estado inicial
        reserva.EstadoReserva = ReservationStatus.Confirmada;

        // guardar en la BD
        await _reservationRepository.CrearReserva(reserva);

        // Notificar al Guest (El que reserva) y al Host (El dueño)
        // Cargamos los datos del Guest porque el objeto reserva que viene del controlador suele traer solo el UserId
        if (reserva.user == null) 
        {
            reserva.user = await _userRepository.GetPorId(reserva.UserId); 
        }

        try {
        // Fire and Forget (_ = ...): Uso el guion bajo antes de la llamada al email porque la rúbrica dice que el fallo del correo no debe detener la transacción principal. Así, el correo se envía "de fondo".

            // email para guest
            _ = _emailService.EnviarNotificacionNuevaReservaAsync(
                reserva.user.Email, 
                reserva.user.Nombre, 
                propiedad.Titulo, 
                reserva.FechaInicio.ToShortDateString(), 
                reserva.FechaFin.ToShortDateString(), false,
                propiedad.PrecioNoche,
                reserva.PrecioTotal
            );
            
            // email para host / propiedad.Owner debe estar mapeado en el Repo
            if (propiedad.Owner != null)
            {
            _ = _emailService.EnviarNotificacionNuevaReservaAsync(
                propiedad.Owner.Email, 
                propiedad.Owner.Nombre, 
                propiedad.Titulo, 
                reserva.FechaInicio.ToShortDateString(), 
                reserva.FechaFin.ToShortDateString(), true,
                propiedad.PrecioNoche,
                reserva.PrecioTotal);
            }
        } catch (Exception ex) { // Esto imprime el error en la terminal de VS Code para poder debugear
    // Pero no detiene el flujo, por lo que la reserva se completa exitosamente.
        Console.WriteLine($"Error no crítico al enviar correos de reserva: {ex.Message}"); }

        return reserva;
    }


    
    public async Task CancelarReserva(int reservationId)
    {
        var reserva = await _reservationRepository.GetPorId(reservationId);
        if (reserva == null) 
        {
            throw new Exception("Reserva no encontrada.");
        }

        // solo se puede cancelar si no ha pasado la fecha de inicio
        if (reserva.FechaInicio <= DateTime.Now)
        {
            throw new Exception("No se puede cancelar una reserva que ya ha iniciado o finalizado. ");
        }
        reserva.EstadoReserva = ReservationStatus.Cancelada;
        await _reservationRepository.ActualizarReserva(reserva);

        // Obtener propiedad para tener el título en el correo
        var propiedad = await _propertyRepository.GetPorId(reserva.PropertyId);

        // Asegurar que el usuario esté cargado para obtener su email
        if (reserva.user == null) reserva.user = await _userRepository.GetPorId(reserva.UserId);

        _ = _emailService.EnviarNotificacionEstadoReservaAsync(
            reserva.user.Email, 
            reserva.user.Nombre, 
            propiedad!.Titulo, "Cancelada");
    }
    
    public async Task CompletarReserva(int reservationId)
    {
        var reserva = await _reservationRepository.GetPorId(reservationId);

        if (reserva == null)
        {
            throw new Exception("Reserva no encontrada. ");
        }

        reserva.EstadoReserva = ReservationStatus.Completada;
        await _reservationRepository.ActualizarReserva(reserva);

        var propiedad = await _propertyRepository.GetPorId(reserva.PropertyId);

        // carga manual del usuario si el repositorio no hizo include
        if (reserva.user == null) reserva.user = await _userRepository.GetPorId(reserva.UserId);

        _ = _emailService.EnviarNotificacionEstadoReservaAsync(
            reserva.user.Email, 
            reserva.user.Nombre, 
            propiedad!.Titulo, "Completada");
    }
    public async Task<List<Reservation>> ObtenerReservas()
    {
        // traeos todos o podria filtrar por usuario tmb
        return await _reservationRepository.GetTodas();
    }
    public async Task<Reservation?> GetPorId(int id)
    {
        return await _reservationRepository.GetPorId(id);
    }
    public async Task<List<Reservation>> ObtenerReservasPorUsuario(int userId)
    {
    // Traemos todas y filtramos donde el UserId coincida
    var todas = await _reservationRepository.GetTodas();
    return todas.Where(r => r.UserId == userId).ToList();
    }   
}