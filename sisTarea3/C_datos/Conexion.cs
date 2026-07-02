using System.Data.SqlClient;

namespace C_datos
{
    public class Conexion
    {
        private string cadenaConexion = "Data Source=.\\SQLEXPRESS;Initial Catalog=BaseTarea3;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
