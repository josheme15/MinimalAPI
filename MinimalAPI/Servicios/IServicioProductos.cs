using MinimalAPI.Data;

namespace MinimalAPI.Servicios
{
    public interface IServicioProductos
    {
        Task CrearProducto(Producto p);
        Task<IEnumerable<Producto>> DameProductos();
        Task<Producto> DameProducto(String sku);

        Task<Producto> ModificarProducto(Producto p);
        Task BorrarProducto(String sku);
    }
}
