using System;
using System.Data;
using System.Data.SqlClient;

namespace C_datos
{
    public class ProveedorDatos
    {
        private Conexion conexion = new Conexion();

        public DataTable ListarProveedores()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT prv_id, prv_nombre, prv_contacto, prv_telefono, prv_correo, prv_imagen_path FROM tbl_proveedor ORDER BY prv_nombre",
                    con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ObtenerProveedorPorId(int prvId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT prv_id, prv_nombre, prv_contacto, prv_telefono, prv_correo, prv_imagen_path FROM tbl_proveedor WHERE prv_id = @id",
                    con);
                da.SelectCommand.Parameters.AddWithValue("@id", prvId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public int InsertarProveedor(string nombre, string contacto, string telefono, string correo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO tbl_proveedor (prv_nombre, prv_contacto, prv_telefono, prv_correo) VALUES (@nombre, @contacto, @telefono, @correo); SELECT SCOPE_IDENTITY();",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@contacto", string.IsNullOrEmpty(contacto) ? (object)DBNull.Value : contacto);
                cmd.Parameters.AddWithValue("@telefono", string.IsNullOrEmpty(telefono) ? (object)DBNull.Value : telefono);
                cmd.Parameters.AddWithValue("@correo", string.IsNullOrEmpty(correo) ? (object)DBNull.Value : correo);
                con.Open();
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        public void ActualizarProveedor(int prvId, string nombre, string contacto, string telefono, string correo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_proveedor SET prv_nombre = @nombre, prv_contacto = @contacto, prv_telefono = @telefono, prv_correo = @correo WHERE prv_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@contacto", string.IsNullOrEmpty(contacto) ? (object)DBNull.Value : contacto);
                cmd.Parameters.AddWithValue("@telefono", string.IsNullOrEmpty(telefono) ? (object)DBNull.Value : telefono);
                cmd.Parameters.AddWithValue("@correo", string.IsNullOrEmpty(correo) ? (object)DBNull.Value : correo);
                cmd.Parameters.AddWithValue("@id", prvId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarImagenProveedor(int prvId, string rutaImagen)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_proveedor SET prv_imagen_path = @ruta WHERE prv_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@ruta", rutaImagen);
                cmd.Parameters.AddWithValue("@id", prvId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool TieneProductosAsociados(int prvId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM tbl_producto WHERE pr_proveedor_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@id", prvId);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public void EliminarProveedor(int prvId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM tbl_proveedor WHERE prv_id = @id",
                    con);
                cmd.Parameters.AddWithValue("@id", prvId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
