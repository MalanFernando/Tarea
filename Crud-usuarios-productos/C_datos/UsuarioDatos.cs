using System;
using System.Data;
using System.Data.SqlClient;

namespace C_datos
{
    // Clase de acceso a datos para la tabla tbl_usuario
    public class UsuarioDatos
    {
        private Conexion conexion = new Conexion();

        // Valida las credenciales del usuario contra la base de datos (usa la función desencripta de SQL)
        // Retorna un DataTable con los datos del usuario si las credenciales son correctas
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

        // Verifica si una cédula ya está registrada en la base de datos
        // excludeUserId permite excluir al propio usuario en ediciones
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

        // Verifica si un correo electrónico ya está registrado en la base de datos
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

        // Inserta un nuevo usuario en la base de datos (la contraseña se encripta con la función encripta de SQL)
        // Retorna el ID del usuario recién creado
        public int RegistrarUsuario(string nombre, string correo, string contrasena, string cedula, string celular, string rol)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO tbl_usuario (usu_nombre, usu_correo, usu_contrasena, usu_cedula, usu_celular, usu_rol) " +
                    "VALUES (@nombre, @correo, dbo.encripta(@contrasena), @cedula, @celular, @rol); SELECT SCOPE_IDENTITY();",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);
                cmd.Parameters.AddWithValue("@cedula", string.IsNullOrEmpty(cedula) ? (object)DBNull.Value : cedula);
                cmd.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(celular) ? (object)DBNull.Value : celular);
                cmd.Parameters.AddWithValue("@rol", rol);
                con.Open();
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        // Obtiene los datos completos de un usuario por su correo (incluye intentos fallidos)
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

        // Guarda el token de recuperación y su fecha de expiración para restablecer la contraseña
        public void GuardarTokenRecuperacion(int usuId, string token, DateTime expiracion)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_token_reset = @token, usu_token_expiracion = @expiracion WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.AddWithValue("@expiracion", expiracion);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Verifica si un token de recuperación es válido y no ha expirado
        // Retorna el ID del usuario asociado al token
        public DataTable ValidarToken(string token)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_id, usu_correo FROM tbl_usuario WHERE usu_token_reset = @token AND usu_token_expiracion > GETDATE()",
                    con);
                da.SelectCommand.Parameters.AddWithValue("@token", token);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Actualiza la contraseña del usuario y limpia los campos de token de recuperación
        public void ActualizarContrasena(int usuId, string nuevaContrasena)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_contrasena = dbo.encripta(@contrasena), usu_token_reset = NULL, usu_token_expiracion = NULL WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@contrasena", nuevaContrasena);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Actualiza el contador de intentos fallidos de inicio de sesión
        public void ActualizarIntentos(int usuId, int intentos)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_intentos = @intentos WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@intentos", intentos);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Bloquea un usuario cambiando su estado a 'B' (Bloqueado)
        public void BloquearUsuario(int usuId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_estado = 'B' WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ObtenerUsuariosPorRol()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_rol AS nombre, COUNT(*) AS total FROM tbl_usuario GROUP BY usu_rol ORDER BY usu_rol", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Obtiene la lista completa de usuarios
        public DataTable ListarUsuarios()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_id, usu_nombre, usu_correo, usu_cedula, usu_celular, usu_rol, usu_estado, usu_intentos " +
                    "FROM tbl_usuario ORDER BY usu_id", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Obtiene un usuario por su ID (incluye ruta de imagen)
        public DataTable ObtenerUsuarioPorId(int usuId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT usu_id, usu_nombre, usu_correo, usu_cedula, usu_celular, usu_rol, usu_estado, usu_imagen_path " +
                    "FROM tbl_usuario WHERE usu_id = @id", con);
                da.SelectCommand.Parameters.AddWithValue("@id", usuId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Actualiza los datos de un usuario (excepto contraseña y estado)
        public void ActualizarUsuario(int usuId, string nombre, string correo, string cedula, string celular, string rol)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_nombre = @nombre, usu_correo = @correo, usu_cedula = @cedula, usu_celular = @celular, usu_rol = @rol WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@cedula", string.IsNullOrEmpty(cedula) ? (object)DBNull.Value : cedula);
                cmd.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(celular) ? (object)DBNull.Value : celular);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Actualiza los datos del perfil del usuario (nombre, cedula, celular)
        public void ActualizarPerfil(int usuId, string nombre, string cedula, string celular)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_nombre = @nombre, usu_cedula = @cedula, usu_celular = @celular WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@cedula", string.IsNullOrEmpty(cedula) ? (object)DBNull.Value : cedula);
                cmd.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(celular) ? (object)DBNull.Value : celular);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Actualiza la ruta de la imagen de perfil del usuario
        public void ActualizarImagenPerfil(int usuId, string rutaImagen)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_imagen_path = @ruta WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@ruta", rutaImagen);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Desencripta y retorna la contraseña de un usuario por su correo
        // Retorna la contraseña en texto plano o null si no existe
        public string ObtenerContrasenaPorCorreo(string correo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT dbo.desencripta(usu_contrasena) FROM tbl_usuario WHERE usu_correo = @correo",
                    con);
                cmd.Parameters.AddWithValue("@correo", correo);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? result.ToString() : null;
            }
        }

        // Cambia el estado de un usuario (A = Activo, I = Inactivo, B = Bloqueado)
        public void CambiarEstadoUsuario(int usuId, string estado)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_usuario SET usu_estado = @estado WHERE usu_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@id", usuId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}