using System.Data.SqlClient;

namespace C_datos
{
    // Clase que proporciona la conexión a la base de datos SQL Server
    public class Conexion
    {
        // Cadena de conexión usando Windows Authentication
        private string cadenaConexion = "Data Source=BUMBLEBEE\\SQLEXPRESS;Initial Catalog=CRUD_Tienda;Integrated Security=True;TrustServerCertificate=True";

        // Retorna una nueva conexión SQL configurada con la cadena de conexión
        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
