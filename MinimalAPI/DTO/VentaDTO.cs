namespace MinimalAPI.DTO
{
    public class VentaDTO
    {
        public ClienteDTO Cliente { get; set; }
        public IEnumerable<ProductoDTO> Producto { get; set; }
        public IEnumerable<String> guids { get; set; }
    }
}
