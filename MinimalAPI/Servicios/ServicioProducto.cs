using Microsoft.Data.SqlClient;
using MinimalAPI.Data;

namespace MinimalAPI.Servicios
{
    public class ServicioProduct : IServicioProductos
    {
        private string CadenaConexion;
        private readonly ILogger<ServicioProduct> log;
        public ServicioProduct(AccesoDatos cadenaConexion, ILogger<ServicioProduct> log)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = log;
        }

        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }
        public async Task CrearProducto(Producto p)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.CommandText = "dbo.ProductosAlta";
                Comm.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 50).Value = p.Nombre;
                Comm.Parameters.Add("@Descripcion", System.Data.SqlDbType.VarChar, 50).Value = p.Descripcion;
                Comm.Parameters.Add("@Precio", System.Data.SqlDbType.Float).Value = p.Precio;   
                Comm.Parameters.Add("@SKU", System.Data.SqlDbType.VarChar, 16).Value = p.SKU;
                await Comm.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error al dar de alta." + ex.Message);
            }
            finally
            {
                if(Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Producto>> DameProductos()
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            List<Producto> listaProductos = new List<Producto>();
            Producto p = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.DameProductos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    p = new Producto
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDouble(reader["Precio"].ToString()),
                        SKU = reader["SKU"].ToString()
                    };

                    listaProductos.Add(p);
                }
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al obtener los productos" + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return listaProductos;
        }

        public async Task<Producto> DameProducto(string sku)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            Producto p = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.DameProductos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@SKU", System.Data.SqlDbType.VarChar, 16).Value = sku;
                SqlDataReader reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    p = new Producto
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDouble(reader["Precio"].ToString()),
                        SKU = reader["SKU"].ToString()
                    };

                }
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al obtener el producto" + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return p;
        }

        public async Task<Producto> ModificarProducto(Producto p)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.ProductosModificar";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 50).Value = p.Nombre;
                Comm.Parameters.Add("@Descripcion", System.Data.SqlDbType.VarChar, 50).Value = p.Descripcion;
                Comm.Parameters.Add("@Precio", System.Data.SqlDbType.Float).Value = p.Precio;
                Comm.Parameters.Add("@SKU", System.Data.SqlDbType.VarChar, 16).Value = p.SKU;
                await Comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al modificar el producto" + ex.Message);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return p;
        }

        public async Task BorrarProducto(String sku)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.ProductoBorrar";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@SKU", System.Data.SqlDbType.VarChar, 16).Value = sku;
                await Comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al borrar el producto" + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

        }
    }
}
