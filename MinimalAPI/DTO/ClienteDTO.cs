using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.DTO
{
    public class ClienteDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public Boolean Activo { get; set; }
    }
}
