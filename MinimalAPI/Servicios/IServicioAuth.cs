using MinimalAPI.DTO;

namespace MinimalAPI.Servicios
{
    public interface IServicioAuth
    {
        Task<String> Login(UsuarioAPIDTO usuarioAPI);
    }
}
