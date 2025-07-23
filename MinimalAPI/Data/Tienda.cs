namespace MinimalAPI.Data
{
    public class Tienda
    {
        public static List<Producto> ListaProductos = new List<Producto>()
        {
            new Producto
            {
                Id = 1,
                Nombre = "Producto 1",
                Descripcion = "Descripción del Producto 1",
                SKU = "SKU-001",
                Precio = 10.99,
                FechaAlta = DateTime.Now
            },
            new Producto{
                Id = 2,
                Nombre = "Producto 2",
                Descripcion = "Descripción del Producto 2",
                SKU = "SKU-002",
                Precio = 20.99,
                FechaAlta = DateTime.Now
            },
            new Producto{
                Id = 3,
                Nombre = "Producto 3",
                Descripcion = "Descripción del Producto 3",
                SKU = "SKU-003",
                Precio = 30.99,
                FechaAlta = DateTime.Now
            }
        };
    }
}
