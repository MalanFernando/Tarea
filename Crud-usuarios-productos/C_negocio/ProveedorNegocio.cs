using System.Data;

namespace C_negocio
{
    public class ProveedorNegocio
    {
        private C_datos.ProveedorDatos proveedorDatos = new C_datos.ProveedorDatos();

        public DataTable ListarProveedores()
        {
            return proveedorDatos.ListarProveedores();
        }

        public DataTable ObtenerProveedorPorId(int prvId)
        {
            return proveedorDatos.ObtenerProveedorPorId(prvId);
        }

        public int InsertarProveedor(string nombre, string contacto, string telefono, string correo)
        {
            return proveedorDatos.InsertarProveedor(nombre, contacto, telefono, correo);
        }

        public void ActualizarProveedor(int prvId, string nombre, string contacto, string telefono, string correo)
        {
            proveedorDatos.ActualizarProveedor(prvId, nombre, contacto, telefono, correo);
        }

        public void ActualizarImagenProveedor(int prvId, string rutaImagen)
        {
            proveedorDatos.ActualizarImagenProveedor(prvId, rutaImagen);
        }

        public bool TieneProductosAsociados(int prvId)
        {
            return proveedorDatos.TieneProductosAsociados(prvId);
        }

        public void EliminarProveedor(int prvId)
        {
            proveedorDatos.EliminarProveedor(prvId);
        }
    }
}
