namespace Application.Interfaces;

public interface IEmailService
{
    Task EnviarCorreoConfirmacionAsync(string email, string nombre, string token);

    Task EnviarNotificacionNuevaReservaAsync(string emailDestino, string nombreReceptor, string tituloPropiedad, string fechaInicio, string fechaFin, bool esHost, decimal precioNoche, decimal totalReserva);
    Task EnviarNotificacionEstadoReservaAsync(string emailDestino, string nombreReceptor, string tituloPropiedad, string nuevoEstado);
}