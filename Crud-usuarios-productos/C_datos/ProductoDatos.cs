using System;
using System.Data;
using System.Data.SqlClient;

namespace C_datos
{
    public class ProductoDatos
    {
        private Conexion conexion = new Conexion();

        public DataTable ListarProductos()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT p.pr_id, p.pr_nombre, p.pr_descripcion, p.pr_precio, p.pr_fecha_creacion, " +
                    "c.cat_nombre AS categoria, prv.prv_nombre AS proveedor, " +
                    "img.pim_path AS imagen_portada " +
                    "FROM tbl_producto p " +
                    "LEFT JOIN tbl_categoria c ON p.pr_categoria_id = c.cat_id " +
                    "LEFT JOIN tbl_proveedor prv ON p.pr_proveedor_id = prv.prv_id " +
                    "OUTER APPLY (SELECT TOP 1 pim.pim_path FROM tbl_producto_imagen pim WHERE pim.pr_id = p.pr_id ORDER BY pim.pim_orden) img " +
                    "ORDER BY p.pr_id DESC", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ObtenerProductoPorId(int prId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT pr_id, pr_nombre, pr_descripcion, pr_precio, pr_categoria_id, pr_proveedor_id " +
                    "FROM tbl_producto WHERE pr_id = @id", con);
                da.SelectCommand.Parameters.AddWithValue("@id", prId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public int InsertarProducto(string nombre, string descripcion, decimal precio, int categoriaId, int usuarioId, int proveedorId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO tbl_producto (pr_nombre, pr_descripcion, pr_precio, pr_categoria_id, pr_usuario_id, pr_proveedor_id) " +
                    "VALUES (@nombre, @descripcion, @precio, @categoria, @usuario, @proveedor); SELECT SCOPE_IDENTITY();", con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@categoria", categoriaId);
                cmd.Parameters.AddWithValue("@usuario", usuarioId);
                cmd.Parameters.AddWithValue("@proveedor", proveedorId);
                con.Open();
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        public void ActualizarProducto(int prId, string nombre, string descripcion, decimal precio, int categoriaId, int proveedorId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE tbl_producto SET pr_nombre = @nombre, pr_descripcion = @descripcion, pr_precio = @precio, " +
                    "pr_categoria_id = @categoria, pr_proveedor_id = @proveedor WHERE pr_id = @id", con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@categoria", categoriaId);
                cmd.Parameters.AddWithValue("@proveedor", proveedorId);
                cmd.Parameters.AddWithValue("@id", prId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarProducto(int prId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM tbl_producto WHERE pr_id = @id", con);
                cmd.Parameters.AddWithValue("@id", prId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ProductoExiste(string nombre, int? excluirId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string sql = "SELECT COUNT(*) FROM tbl_producto WHERE LOWER(pr_nombre) = LOWER(@nombre)";
                if (excluirId.HasValue)
                    sql += " AND pr_id != @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                if (excluirId.HasValue)
                    cmd.Parameters.AddWithValue("@id", excluirId.Value);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public DataTable ObtenerProductosPorCategoria()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT c.cat_nombre AS nombre, COUNT(p.pr_id) AS total " +
                    "FROM tbl_categoria c " +
                    "LEFT JOIN tbl_producto p ON c.cat_id = p.pr_categoria_id " +
                    "GROUP BY c.cat_nombre ORDER BY c.cat_nombre", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ObtenerProductosPorProveedor()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT ISNULL(prv.prv_nombre, '(Sin proveedor)') AS nombre, COUNT(p.pr_id) AS total " +
                    "FROM tbl_producto p " +
                    "LEFT JOIN tbl_proveedor prv ON p.pr_proveedor_id = prv.prv_id " +
                    "GROUP BY prv.prv_nombre ORDER BY prv.prv_nombre", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarCategorias()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT cat_id, cat_nombre FROM tbl_categoria ORDER BY cat_nombre", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarImagenesPorProducto(int prId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT pim_id, pim_path, pim_orden FROM tbl_producto_imagen WHERE pr_id = @id ORDER BY pim_orden",
                    con);
                da.SelectCommand.Parameters.AddWithValue("@id", prId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertarImagenProducto(int prId, string path, int orden)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO tbl_producto_imagen (pr_id, pim_path, pim_orden) VALUES (@prId, @path, @orden)", con);
                cmd.Parameters.AddWithValue("@prId", prId);
                cmd.Parameters.AddWithValue("@path", path);
                cmd.Parameters.AddWithValue("@orden", orden);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarImagenProducto(int pimId)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM tbl_producto_imagen WHERE pim_id = @id", con);
                cmd.Parameters.AddWithValue("@id", pimId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
