using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.DTO
{
    public class ProductoDTO
    {
        [Required]
        public String Nombre { get; set; }
        [Required]
        public String Descripcion { get; set; }
        [Required]
        [Range(5,500,
            ErrorMessage = "Valor {0} debe estar entre {1} y {2}.")]
        public double Precio { get; set; }
        [Required]
        public String SKU { get; set; }

    }
}
