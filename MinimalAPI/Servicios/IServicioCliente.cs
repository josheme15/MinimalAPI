using MinimalAPI.Data;

namespace MinimalAPI.Servicios
{
    public interface IServicioCliente
    {
        Task CrearCliente(Cliente c);
        Task<IEnumerable<Cliente>> DameClientes();

        Task<Cliente> DameCliente(String email);

        Task<Cliente> ModificarCliente(Cliente c);

        Task BorrarCliente(string email);
    }
}
