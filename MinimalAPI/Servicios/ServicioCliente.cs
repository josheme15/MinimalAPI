using Microsoft.Data.SqlClient;
using MinimalAPI.Data;
using System.Data;

namespace MinimalAPI.Servicios
{
    public class ServicioCliente : IServicioCliente
    {
        private string CadenaConexion;
        private readonly ILogger<ServicioCliente> log;
        public ServicioCliente(AccesoDatos cadenaConexion, ILogger<ServicioCliente> log)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = log;
        }

        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }
        public async Task CrearCliente(Cliente c)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.CommandText = "dbo.ClienteAlta";
                Comm.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = c.Email;
                Comm.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = c.Password;
                await Comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error al dar de alta." + ex.Message);
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

        public async Task<IEnumerable<Cliente>> DameClientes()
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            List<Cliente> listaClientes = new List<Cliente>();
            Cliente c = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.DameClientes";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    c = new Cliente
                    {
                        Email = reader["Email"].ToString(),
                        Password = reader["Pass"].ToString(),
                        FechaAlta = Convert.ToDateTime(reader["FechaAlta"]),
                        FechaBaja = reader["FechaBaja"] != DBNull.Value ?
                        Convert.ToDateTime(reader["FechaBaja"]) :
                        (DateTime?)null

                    };

                    listaClientes.Add(c);
                }
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al obtener los clientes" + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return listaClientes;
        }

        public async Task<Cliente> DameCliente(string email)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            Cliente c = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.DameClientes";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 50).Value = email;
                SqlDataReader reader = await Comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    c = new Cliente
                    {
                        Email = reader["Email"].ToString(),
                        Password = reader["Pass"].ToString(),
                        FechaAlta = Convert.ToDateTime(reader["FechaAlta"]),
                        FechaBaja = reader["FechaBaja"] != DBNull.Value ?
                        Convert.ToDateTime(reader["FechaBaja"]) :
                        (DateTime?)null
                    };

                }
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al obtener el cliente" + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return c;
        }

        public async Task<Cliente> ModificarCliente(Cliente c)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.ClientesModificar";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 50).Value = c.Email;
                Comm.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 50).Value = c.Password;
                await Comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al modificar el cliente" + ex);
            }
            finally
            {
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return c;
        }

        public async Task BorrarCliente(string email)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.ClienteBorrar";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 50).Value = email;
                await Comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un error  al borrar el cliente" + ex);
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
