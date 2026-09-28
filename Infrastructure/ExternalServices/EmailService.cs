using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.ExternalServices;

// Al usar public async Task<IActionResult>, permites que el servidor de .NET no se quede bloqueado esperando a que la base de datos responda o que el correo se envíe.
public class EmailService : IEmailService
{
   private readonly IConfiguration _config;
   // inyectamos IConfig para leer las credenciales del appsettings.json

   public EmailService(IConfiguration config)
    {
        _config = config;
    } 
    public async Task EnviarCorreoConfirmacionAsync(string emailDestino, string nombreUsuario, string tokenConfirmacion)
{
    // Extraemos los valores con cuidado
    var host = _config["Email:Host"] ?? "localhost";
    var portValue = _config["Email:Port"] ?? "1025";
    var user = _config["Email:User"] ?? "airbnbmf@mail.com";
    var pass = _config["Email:Pass"] ?? "";

    int port = int.Parse(portValue);

    string asunto = "Bienvenido - Confirma tu cuenta";
string cuerpo = $@"
    <div style='font-family: Arial, sans-serif; border: 1px solid #eee; padding: 25px; max-width: 500px; border-radius: 10px;'>
        <h1 style='color: #003366; font-size: 28px;'>¡Hola, {nombreUsuario}!</h1>
        
        <p style='color: #333; font-size: 16px;'>
            Gracias por registrarte. Para activar tu cuenta, utiliza el siguiente código de seguridad:
        </p>
        
        <div style='background: #f8f9fa; padding: 15px; text-align: center; border-radius: 5px; margin: 20px 0;'>
            <h2 style='letter-spacing: 5px; font-size: 32px; color: #333; margin: 0;'>{tokenConfirmacion}</h2>
        </div>

        <p style='color: #D32F2F; font-size: 14px; font-weight: bold;'>
          ⚠️ Este código expirará en 24 horas.
        </p>

        <hr style='border: 0; border-top: 1px solid #eee; margin-top: 30px;'>
        
        <p style='color: #999; font-size: 11px; margin-top: 10px;'>
            Si no solicitaste este correo, puedes ignorarlo con total seguridad. Nadie más tiene acceso a tu cuenta en este momento.
        </p>
    </div>";

    // Configuración para Mailpit
    using var client = new SmtpClient(host, port)
    {
        // Mailpit no requiere credenciales reales ni SSL
        Credentials = new NetworkCredential(user, pass),
        EnableSsl = false 
    };

    var mailMessage = new MailMessage
    {
        From = new MailAddress(user, "Airbnb MF"),
        Subject = asunto,
        Body = cuerpo,
        IsBodyHtml = true
    };
    
    mailMessage.To.Add(emailDestino);

    // Enviamos
    await client.SendMailAsync(mailMessage);
}
        
        public async Task EnviarNotificacionNuevaReservaAsync(string emailDestino, string nombreReceptor, string tituloPropiedad, string fechaInicio, string fechaFin, bool esHost, decimal precioNoche, decimal totalReserva)
{
    string rolMensaje = esHost ? "¡Tienes una nueva reserva!" : "¡Tu reserva ha sido confirmada!";
    string detalleRol = esHost ? $"El usuario ha reservado tu propiedad" : $"Has reservado con éxito la propiedad";

    string asunto = $"Airbnb MF - {rolMensaje}";
    string cuerpo = $@"
        <div style='font-family: Arial, sans-serif; border: 1px solid #eee; padding: 25px; max-width: 500px; border-radius: 10px;'>
            <h1 style='color: #003366; font-size: 24px;'>¡Hola, {nombreReceptor}!,</h1>
            <h2 style='color: #FF385C;'>{rolMensaje}</h2>
            <p style='color: #333; font-size: 16px;'>
                {detalleRol}: <strong>{tituloPropiedad}</strong>
            </p>
            <div style='background: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                <p style='margin: 5px 0;'><strong>Desde:</strong> {fechaInicio}</p>
    </ul>
                <p style='margin: 5px 0;'><strong>Hasta:</strong> {fechaFin}</p>
                <hr style='border: 0; border-top: 1px solid #ddd; margin: 10px 0;'>
                <p style='margin: 5px 0;'><strong>Precio por noche:</strong> ${precioNoche:N2}</p>
                <p style='margin: 5px 0; font-size: 18px;'><strong>Total a pagar:</strong> <span style='color: #2E7D32; font-weight: bold;'>${totalReserva:N2}</span></p>
            </div>
            <p style='color: #999; font-size: 11px;'>Gracias por usar Airbnb MF.</p>
        </div>";

    await EnviarEmailBaseAsync(emailDestino, asunto, cuerpo);
}

public async Task EnviarNotificacionEstadoReservaAsync(string emailDestino, string nombreReceptor, string tituloPropiedad, string nuevoEstado)
{
    string colorEstado = nuevoEstado == "Cancelada" ? "#D32F2F" : "#2E7D32";
    string mensajeExtra = nuevoEstado == "Completada" ? "¡Esperamos que hayas disfrutado tu estancia! Ya puedes dejar una reseña." : "La reserva ha sido actualizada.";

    string asunto = $"Actualización de Reserva: {nuevoEstado}";
    string cuerpo = $@"
        <div style='font-family: Arial, sans-serif; border: 1px solid #eee; padding: 25px; max-width: 500px; border-radius: 10px;'>
            <h1 style='color: #003366; font-size: 24px;'>¡Hola, {nombreReceptor}!</h1>
            <p>Tu reserva para <strong>{tituloPropiedad}</strong> ahora está:</p>
            <h2 style='color: {colorEstado};'>{nuevoEstado}</h2>
            <p style='color: #333;'>{mensajeExtra}</p>
        </div>";

    await EnviarEmailBaseAsync(emailDestino, asunto, cuerpo);
}

// Método privado para no repetir la lógica de SmtpClient
private async Task EnviarEmailBaseAsync(string emailDestino, string asunto, string cuerpo)
{
        try
        {        
    var host = _config["Email:Host"] ?? "localhost";
    var port = int.Parse(_config["Email:Port"] ?? "1025");
    
    using var client = new SmtpClient(host, port) { EnableSsl = false };
    var mailMessage = new MailMessage
    {
        From = new MailAddress(_config["Email:User"] ?? "airbnbmf@mail.com", "Airbnb MF"),
        Subject = asunto,
        Body = cuerpo,
        IsBodyHtml = true
    };
    mailMessage.To.Add(emailDestino);
    await client.SendMailAsync(mailMessage);
}
catch (Exception ex)
        {
        // Si el correo falla, solo lo escribimos en la consola pero NO matamos el proceso
        Console.WriteLine($"--> Error enviando correo: {ex.Message}");
        }
}
}