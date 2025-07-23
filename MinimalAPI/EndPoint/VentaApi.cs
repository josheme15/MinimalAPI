using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Data;
using MinimalAPI.DTO;
using MinimalAPI.Servicios;
using System.Threading.Tasks;

namespace MinimalAPI.EndPoint
{
    public static class VentaApi
    {
        public static async Task ConfigurarVentaEndpoints(this WebApplication app)
        {
            app.MapPost("/api/venta", CrearVenta)
                .RequireAuthorization().WithName("CrearVenta");

            app.MapGet("/api/venta", DameVentas)
                .RequireAuthorization().WithName("DameVentas");

            app.MapGet("/api/venta/{email}", DameVenta)
                .RequireAuthorization().WithName("DameVenta");

            app.MapPut("api/venta/{guid}/{sku}", ModificarVenta)
                .RequireAuthorization().WithName("ModificarVenta");

            app.MapDelete("/api/venta/{guid}", BorrarVenta)
                .RequireAuthorization().WithName("BorrarVentar");
        }

        private async static Task<IResult> CrearVenta([FromBody] VentaDTO ventaDTO,
            IServicioVenta servicioVenta)
        {
            Venta venta = null;
            venta = new Venta
            {
                email = ventaDTO.Cliente.Email,
                skus = ventaDTO.Producto.Select(p => p.SKU).ToList()
            };

            await servicioVenta.CrearVenta(venta);
            return Results.Ok();
        }

        private async static Task<IResult> DameVentas(IServicioVenta servicioVenta)
        {
            var listaVentas = (await servicioVenta.DameVentas())
                .Select(p => p.convertirDTO());
            return Results.Ok(listaVentas);
        }

        private async static Task<IResult> DameVenta(String email, IServicioVenta servicioVenta)
        {
            Venta v = await servicioVenta.DameVenta(email);
            if (v == null)
                return Results.NotFound("Sin ventas");
            return Results.Ok(v.convertirDTO());
        } 

        private async static Task<IResult> ModificarVenta(string guid, string sku, IServicioVenta servicioVenta)
        {
            if(guid == String.Empty)
            {
                return Results.BadRequest("El guid no puede esta vacio.");
            }
            if(sku == String.Empty)
            {
                return Results.BadRequest("El sku no puede esta vacio.");
            }

            await servicioVenta.ModificarVenta(guid, sku);
            return Results.Ok();
        }
        
        private async static Task<IResult> BorrarVenta(string Guid,
            IServicioVenta servicioVenta)
        {
            await servicioVenta.BorrarVenta(Guid);
            return Results.NoContent();
        }
    }
}
