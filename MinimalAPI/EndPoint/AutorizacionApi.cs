using Microsoft.AspNetCore.Mvc;
using MinimalAPI.DTO;
using MinimalAPI.Servicios;

namespace MinimalAPI.EndPoint
{
    public static class AutorizacionApi
    {
        public static void ConfigurarLoginEndpoints(this WebApplication app)
        {
            app.MapPost("api/login", Login)
                .AllowAnonymous().WithName("Login");
        }

        private async static Task<IResult> Login([FromBody] UsuarioAPIDTO usuarioDTO, IServicioAuth servicioAuth) 
        {
            string token = await servicioAuth.Login(usuarioDTO);
            if (token == String.Empty)
                return Results.NotFound("Usuario no encontrado.");
            return Results.Ok(token);
        }

}
}
