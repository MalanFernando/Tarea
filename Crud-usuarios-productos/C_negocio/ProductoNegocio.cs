using System;
using System.Data;

namespace C_negocio
{
    public class ProductoNegocio
    {
        private C_datos.ProductoDatos productoDatos = new C_datos.ProductoDatos();

        public DataTable ListarProductos()
        {
            return productoDatos.ListarProductos();
        }

        public DataTable ObtenerProductoPorId(int prId)
        {
            return productoDatos.ObtenerProductoPorId(prId);
        }

        public int InsertarProducto(string nombre, string descripcion, decimal precio, int categoriaId, int usuarioId, int proveedorId)
        {
            return productoDatos.InsertarProducto(nombre, descripcion, precio, categoriaId, usuarioId, proveedorId);
        }

        public void ActualizarProducto(int prId, string nombre, string descripcion, decimal precio, int categoriaId, int proveedorId)
        {
            productoDatos.ActualizarProducto(prId, nombre, descripcion, precio, categoriaId, proveedorId);
        }

        public void EliminarProducto(int prId)
        {
            productoDatos.EliminarProducto(prId);
        }

        public bool ProductoExiste(string nombre, int? excluirId)
        {
            return productoDatos.ProductoExiste(nombre, excluirId);
        }

        public DataTable ObtenerProductosPorCategoria()
        {
            return productoDatos.ObtenerProductosPorCategoria();
        }

        public DataTable ObtenerProductosPorProveedor()
        {
            return productoDatos.ObtenerProductosPorProveedor();
        }

        public DataTable ListarCategorias()
        {
            return productoDatos.ListarCategorias();
        }

        public DataTable ListarImagenesPorProducto(int prId)
        {
            return productoDatos.ListarImagenesPorProducto(prId);
        }

        public void InsertarImagenProducto(int prId, string path, int orden)
        {
            productoDatos.InsertarImagenProducto(prId, path, orden);
        }

        public void EliminarImagenProducto(int pimId)
        {
            productoDatos.EliminarImagenProducto(pimId);
        }
    }
}
