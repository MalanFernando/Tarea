using System;
using System.Data;
using System.Data.SqlClient;

namespace C_datos
{
    public class UsuarioDatos
    {
        private Conexion conexion = new Conexion();

        public DataTable ValidarUsuario(string correo, string contrasena)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_id, usu_nombre, usu_correo, usu_rol, usu_estado, usu_intentos " +
                    "FROM tbl_usuario WHERE usu_correo = @correo AND dbo.desencripta(usu_contrasena) = @contrasena",
                    con);
                da.SelectCommand.Parameters.AddWithValue("@correo", correo);
                da.SelectCommand.Parameters.AddWithValue("@contrasena", contrasena);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool CedulaExiste(string cedula, int? excludeUserId = null)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "SELECT COUNT(*) FROM tbl_usuario WHERE usu_cedula = @cedula";
                if (excludeUserId.HasValue)
                    query += " AND usu_id != @excludeId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cedula", cedula);
                if (excludeUserId.HasValue)
                    cmd.Parameters.AddWithValue("@excludeId", excludeUserId.Value);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool CorreoExiste(string correo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM tbl_usuario WHERE usu_correo = @correo", con);
                cmd.Parameters.AddWithValue("@correo", correo);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public int RegistrarUsuario(string nombre, string correo, string contrasena, string cedula, string celular, string rol, DateTime? fechaNacimiento = null, string correoSecundario = null)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO tbl_usuario (usu_nombre, usu_correo, usu_correo_secundario, usu_contrasena, usu_cedula, usu_celular, usu_rol, usu_fecha_nacimiento) " +
                    "VALUES (@nombre, @correo, @correoSec, dbo.encripta(@contrasena), @cedula, @celular, @rol, @fechaNac); SELECT SCOPE_IDENTITY();",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@correoSec", string.IsNullOrEmpty(correoSecundario) ? (object)DBNull.Value : correoSecundario);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);
                cmd.Parameters.AddWithValue("@cedula", string.IsNullOrEmpty(cedula) ? (object)DBNull.Value : cedula);
                cmd.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(celular) ? (object)DBNull.Value : celular);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@fechaNac", fechaNacimiento.HasValue ? (object)fechaNacimiento.Value : DBNull.Value);
                con.Open();
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        public DataTable ObtenerUsuarioPorCorreo(string correo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_id, usu_nombre, usu_correo, usu_rol, usu_estado, usu_intentos " +
                    "FROM tbl_usuario WHERE usu_correo = @correo", con);
                da.SelectCommand.Parameters.AddWithValue("@correo", correo);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void ActualizarIntentos(int usuId, int intentos)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_intentos = @intentos WHERE usu_id = @id", con);
                cmd.Parameters.AddWithValue("@intentos", intentos);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void BloquearUsuario(int usuId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_estado = 'B' WHERE usu_id = @id", con);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ListarUsuarios()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_id, usu_nombre, usu_correo, usu_correo_secundario, usu_cedula, usu_celular, usu_rol, usu_estado, usu_intentos, usu_fecha_nacimiento " +
                    "FROM tbl_usuario ORDER BY usu_id", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ObtenerUsuarioPorId(int usuId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_id, usu_nombre, usu_correo, usu_correo_secundario, usu_cedula, usu_celular, usu_rol, usu_estado, usu_fecha_nacimiento " +
                    "FROM tbl_usuario WHERE usu_id = @id", con);
                da.SelectCommand.Parameters.AddWithValue("@id", usuId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void ActualizarUsuario(int usuId, string nombre, string correo, string cedula, string celular, string rol, DateTime? fechaNacimiento = null, string correoSecundario = null)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_nombre = @nombre, usu_correo = @correo, usu_correo_secundario = @correoSec, usu_cedula = @cedula, usu_celular = @celular, usu_rol = @rol, usu_fecha_nacimiento = @fechaNac WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@correoSec", string.IsNullOrEmpty(correoSecundario) ? (object)DBNull.Value : correoSecundario);
                cmd.Parameters.AddWithValue("@cedula", string.IsNullOrEmpty(cedula) ? (object)DBNull.Value : cedula);
                cmd.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(celular) ? (object)DBNull.Value : celular);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@fechaNac", fechaNacimiento.HasValue ? (object)fechaNacimiento.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarPerfil(int usuId, string nombre, string cedula, string celular, DateTime? fechaNacimiento = null, string correoSecundario = null)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_nombre = @nombre, usu_correo_secundario = @correoSec, usu_cedula = @cedula, usu_celular = @celular, usu_fecha_nacimiento = @fechaNac WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@correoSec", string.IsNullOrEmpty(correoSecundario) ? (object)DBNull.Value : correoSecundario);
                cmd.Parameters.AddWithValue("@cedula", string.IsNullOrEmpty(cedula) ? (object)DBNull.Value : cedula);
                cmd.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(celular) ? (object)DBNull.Value : celular);
                cmd.Parameters.AddWithValue("@fechaNac", fechaNacimiento.HasValue ? (object)fechaNacimiento.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string ObtenerContrasenaPorCorreo(string correo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT dbo.desencripta(usu_contrasena) FROM tbl_usuario WHERE usu_correo = @correo", con);
                cmd.Parameters.AddWithValue("@correo", correo);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? result.ToString() : null;
            }
        }

        public void CambiarEstadoUsuario(int usuId, string estado)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_estado = @estado WHERE usu_id = @id", con);
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarContrasena(int usuId, string nuevaContrasena)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_contrasena = dbo.encripta(@contrasena) WHERE usu_id = @id", con);
                cmd.Parameters.AddWithValue("@contrasena", nuevaContrasena);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
