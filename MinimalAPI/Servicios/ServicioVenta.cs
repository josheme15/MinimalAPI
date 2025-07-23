using Microsoft.Data.SqlClient;
using MinimalAPI.Data;
using System.Data;
using System.Transactions;

namespace MinimalAPI.Servicios
{
    public class ServicioVenta : IServicioVenta
    {
        private string CadenaConexion;
        private readonly ILogger<ServicioVenta> log;
        public ServicioVenta(AccesoDatos cadenaConexion, ILogger<ServicioVenta> log)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = log;
        }

        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }
        public async Task CrearVenta(Venta v)
        {
            SqlConnection sqlConexion = conexion();
            SqlTransaction transaction = null;
            try
            {
                await sqlConexion.OpenAsync();
                transaction = sqlConexion.BeginTransaction();
                SqlCommand Comm = sqlConexion.CreateCommand();
                Comm.Transaction = transaction;
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.CommandText = "dbo.VentaAlta";
                foreach(string sku in v.skus){
                    Comm.Parameters.Clear();
                    Comm.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = v.email;
                    Comm.Parameters.Add("@sku", SqlDbType.VarChar, 16).Value = sku;
                    await Comm.ExecuteNonQueryAsync();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                if(transaction != null)
                    transaction.Rollback();
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error al dar de alta a la venta." + ex);
            }
            finally
            {
                if (sqlConexion.State == ConnectionState.Open)
                    sqlConexion.Close();
                sqlConexion.Dispose();
            }
        }

        public async Task<IEnumerable<Venta>> DameVentas()
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            List<Venta> ventas = new List<Venta>();
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.DameVentas";
                Comm.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    string email = reader["email"].ToString();
                    string sku = reader["Sku"].ToString();
                    string guid = reader["identificador"].ToString();

                    Venta v = ventas.FirstOrDefault(venta => venta.email == email);
                    if (v == null)
                    {
                        v = new Venta();
                        v.email = email;
                        v.skus = new List<string>();
                        v.guids = new List<string>();
                        ventas.Add(v);
                    }
                    ((List<string>)v.skus).Add(sku);
                    ((List<string>)v.guids).Add(guid);
                }
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error al obtener las ventas." + ex);
            }
            finally
            {
                if(Comm != null)
                    Comm.Dispose();

                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return ventas;
        }

        public async Task<Venta> DameVenta(string email)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            Venta venta = new Venta();
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.DameVentas";
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = email;
                SqlDataReader reader = await Comm.ExecuteReaderAsync();
                
                venta.skus = new List<string>();
                venta.guids = new List<string>();
                while (reader.Read())
                {
                    venta.email = reader["email"].ToString();
                    ((List<string>)venta.skus).Add(reader["Sku"].ToString());
                    ((List<string>)venta.guids).Add(reader["identificador"].ToString());
                }
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error al obtener la venta." + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();

                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return venta;
        }

        public async Task ModificarVenta(string guid, string nuevoSku)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.VentasModificar";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@sku", System.Data.SqlDbType.VarChar, 16).Value = nuevoSku;
                Comm.Parameters.Add("@identificador", System.Data.SqlDbType.VarChar, 50).Value = guid;
                await Comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al modificar la venta" + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            await Task.CompletedTask;
        }

        public async Task BorrarVenta(string guid)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.VentasBorrar";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@guid", System.Data.SqlDbType.VarChar, 50).Value = guid;
                await Comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al borrar la venta" + ex);
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
