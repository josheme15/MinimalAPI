namespace MinimalAPI.Data
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string SKU { get; set; }
        public double Precio { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }
    }
}
