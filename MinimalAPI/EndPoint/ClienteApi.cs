using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Data;
using MinimalAPI.DTO;
using MinimalAPI.Servicios;

namespace MinimalAPI.EndPoint
{
    public static class ClienteApi
    {
        public static void ConfigurarClienteEndpoints(this WebApplication app)
        {
            app.MapPost("/api/clientes", CrearCliente)
                .RequireAuthorization().WithName("CrearCliente");

            app.MapGet("/api/clientes", DameClientes)
                .RequireAuthorization().WithName("DameClientes");

            app.MapGet("/api/clientes/{email}", DameCliente)
                .RequireAuthorization().WithName("DameCliente");

            app.MapPut("/api/clientes", ModificarCliente)
                .RequireAuthorization().WithName("ModificarCliente");

            app.MapDelete("/api/clientes/{email}", BorrarCliente)
                .RequireAuthorization().WithName("BorrarCliente");
        }

        private async static Task<IResult> CrearCliente
            ([FromBody] ClienteDTO clienteDTO, IServicioCliente servicioCliente)
        {
            Cliente cliente = null;
            cliente = new Cliente
            {
                Email = clienteDTO.Email,
                Password = clienteDTO.Password
            };
            await servicioCliente.CrearCliente(cliente);
            return Results.Ok();
        }

        private async static Task<IResult> DameClientes(IServicioCliente servicioCliente)
        {
            var listaClientes = (await servicioCliente.DameClientes())
                .Select(c => c.convertirDTO());
            return Results.Ok(listaClientes);
        }

        private async static Task<IResult> DameCliente(string email, IServicioCliente servicioCliente)
        {
            Cliente c = await servicioCliente.DameCliente(email);
            if(c== null)
                return Results.NotFound("Cliente no encontrado");
            return Results.Ok(c.convertirDTO());
        }

        private async static Task<IResult> ModificarCliente([FromBody] ClienteDTO clienteDTO, IServicioCliente servicioCliente)
        {
            if (clienteDTO.Email == String.Empty)
            {
                return Results.BadRequest("El email no puede estar vacío");
            }
            Cliente cliente = null;
            cliente = new Cliente
            {
                Email = clienteDTO.Email,
                Password = clienteDTO.Password
            };

            Cliente c = await servicioCliente.ModificarCliente(cliente);
            return Results.Ok(c.convertirDTO());
        }

        private async static Task<IResult> BorrarCliente(string email, IServicioCliente servicioCliente)
        {
            await servicioCliente.BorrarCliente(email);
            return Results.NoContent();
        }
    }
}
