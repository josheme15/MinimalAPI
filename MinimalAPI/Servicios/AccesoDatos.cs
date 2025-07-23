namespace MinimalAPI.Servicios
{
    public class AccesoDatos
    {
        private string cadenaConexionSQL;
        public string CadenaConexionSQL
        {
            get => cadenaConexionSQL;
        }

        public AccesoDatos(string ConexionSql)
        {
            cadenaConexionSQL = ConexionSql;
        }
    }
}
