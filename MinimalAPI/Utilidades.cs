using MinimalAPI.Data;
using MinimalAPI.DTO;

namespace MinimalAPI
{
    public static class Utilidades
    {
        public static ProductoDTO convertirDTO(this Producto p)
        {
            if (p != null)
            {
                return new ProductoDTO
                {
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    SKU = p.SKU
                };
            }
            return null;
        }

        public static ClienteDTO convertirDTO(this Cliente c)
        {
            if (c != null)
            {
                return new ClienteDTO
                {
                    Email = c.Email,
                    Password = c.Password,
                    Activo = c.FechaBaja == null ? true : false
                };
            }
            return null;
        }

        public static VentaDTO convertirDTO(this Venta v)
        {
            if (v != null) 
            {
                VentaDTO aux = new VentaDTO
                {
                    Cliente = new ClienteDTO(),
                    Producto = new List<ProductoDTO>()
                };
                aux.Cliente.Email = v.email;
                foreach (string sku in v.skus)
                {
                    ProductoDTO p = new ProductoDTO();
                    p.SKU = sku;
                    ((List<ProductoDTO>)aux.Producto).Add(p);
                }

                aux.guids = new List<string>(v.guids);
                return aux;
            }
            return null;
        }
        
    }
}
