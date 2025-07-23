using MinimalAPI.Data;

namespace MinimalAPI.Servicios
{
    public interface IServicioVenta
    {
        Task CrearVenta(Venta v);
        Task<IEnumerable<Venta>> DameVentas();
        Task<Venta> DameVenta(string email);
        Task ModificarVenta(string guid, string nuevoSku);
        Task BorrarVenta(string guid);
    }
}
