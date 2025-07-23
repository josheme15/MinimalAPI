using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Data;
using MinimalAPI.DTO;
using MinimalAPI.Servicios;

namespace MinimalAPI.EndPoint
{
    public static class ProductoApi
    {
        public static void ConfigurarProductoEndpoints(this WebApplication app)
        {
            app.MapGet("/api/productos", DameProductos)
                .RequireAuthorization().WithName("DameProductos");
            app.MapGet("/api/producto/{sku}", DameProducto)
                .RequireAuthorization().WithName("DameProducto");
            app.MapPost("/api/productos", CrearProducto)
                .RequireAuthorization().WithName("CrearProducto");
            app.MapPut("/api/productos", ModificarProducto)
                .RequireAuthorization().WithName("ModificarProducto");
            app.MapDelete("/api/productos/{sku}", BorrarProducto)
                .RequireAuthorization().WithName("BorrarProducto");
        }

        private async static Task<IResult> DameProductos(IServicioProductos servicioProductos)
        {
            var listaProductos = (await servicioProductos.DameProductos())
                .Select(p => p.convertirDTO());
            return Results.Ok(listaProductos);
        }

        private async static Task<IResult> DameProducto(string sku, IServicioProductos servicioProductos)
        {
            Producto p = await servicioProductos.DameProducto(sku);

            if (p == null)
                return Results.NotFound("No se encontró el producto");
            return Results.Ok(p.convertirDTO());
        }

        private async static Task<IResult> CrearProducto([FromBody] ProductoDTO productoDto, IServicioProductos servicioProductos)
        {

            Producto producto = null;
            producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                Precio = productoDto.Precio,
                SKU = productoDto.SKU
            };

            await servicioProductos.CrearProducto(producto);
            return Results.Ok();
        }

        private async static Task<IResult> ModificarProducto([FromBody] ProductoDTO productoDto, IServicioProductos servicioProductos)
        {
            if (productoDto.SKU == String.Empty)
            {
                return Results.BadRequest("El SKU no puede estar vacío");
            }
            Producto producto = null;
            producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                Precio = productoDto.Precio,
                SKU = productoDto.SKU
            };

            Producto p = await servicioProductos.ModificarProducto(producto);
            return Results.Ok(p.convertirDTO);
        }

        private async static Task<IResult> BorrarProducto(string Sku, IServicioProductos servicioProductos)
        {
            await servicioProductos.BorrarProducto(Sku);
            return Results.NoContent();
        }
        
}
}
