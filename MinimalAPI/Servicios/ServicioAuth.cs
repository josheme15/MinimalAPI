
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using MinimalAPI.Data;
using MinimalAPI.DTO;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalAPI.Servicios
{
    public class ServicioAuth : IServicioAuth
    {
        private string CadenaConexion;
        private readonly IConfiguration configuration;
        private readonly ILogger<ServicioAuth> log;
        public ServicioAuth(AccesoDatos cadenaConexion, IConfiguration configuration, ILogger<ServicioAuth> log)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.configuration = configuration;
            this.log = log;
        }

        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }

        public async Task<string> Login(UsuarioAPIDTO usuarioAPI)
        {
            UsuarioApi Usuario = null;
            string token = string.Empty;
            Usuario = await AutenticarUsuarioAsync(usuarioAPI);
            if(Usuario == null)
                throw new Exception("Usuario o contraseña incorrectos.");
            else
                token = GenerarTokenJWT(Usuario);
            return token;
        }

        private async Task<UsuarioApi> AutenticarUsuarioAsync(UsuarioAPIDTO usuarioLogin)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            UsuarioApi p = null;
            try
            {
                await sqlConexion.OpenAsync();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioAPIObtener";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@UsuarioApi", SqlDbType.VarChar, 50).Value = usuarioLogin.Usuario;
                Comm.Parameters.Add("@PassApi", SqlDbType.VarChar, 50).Value = usuarioLogin.passAPI;
                SqlDataReader reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                {
                    p = new UsuarioApi
                    {
                        Usuario = reader["UsuarioApi"].ToString(),
                        Email = reader["EmailUsuario"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                log.LogError("ERROR: " + ex.ToString());
                throw new Exception("Se produjo un erro al obtener usuario." + ex);
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

        private string GenerarTokenJWT(UsuarioApi usuarioInfo)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:ClaveSecreta"]));
            var _signingCredentials = new SigningCredentials(
                _symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _Header = new JwtHeader(_signingCredentials);
            var _Claims = new[] {
                new Claim("usuario", usuarioInfo.Usuario),
                new Claim("email", usuarioInfo.Email),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Email),
            };

            var _Payload = new JwtPayload(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: _Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1));

            var _Token = new JwtSecurityToken(_Header, _Payload);
            string token = new JwtSecurityTokenHandler().WriteToken(_Token);
            return token;
        }

    }
}
