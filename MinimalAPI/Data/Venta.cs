namespace MinimalAPI.Data
{
    public class Venta
    {
        public string email { get; set; }
        public IEnumerable<String> skus { get; set; }
        public IEnumerable<String> guids { get; set; }
    }
}
