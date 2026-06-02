namespace C_presentacion
{
    public partial class AdminProductos
    {
        protected global::System.Web.UI.WebControls.Label lblMensaje;
        protected global::System.Web.UI.WebControls.HyperLink hlInicio;
        protected global::System.Web.UI.WebControls.GridView gvProductos;
        protected global::System.Web.UI.WebControls.Button btnNuevo;
        protected global::System.Web.UI.WebControls.Panel pnlFormulario;
        protected global::System.Web.UI.WebControls.Label lblTituloFormulario;
        protected global::System.Web.UI.WebControls.HiddenField hfProductoId;
        protected global::System.Web.UI.WebControls.Label lblNombre;
        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvNombre;
        protected global::System.Web.UI.WebControls.Label lblDescripcion;
        protected global::System.Web.UI.WebControls.TextBox txtDescripcion;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvDescripcion;
        protected global::System.Web.UI.WebControls.Label lblPrecio;
        protected global::System.Web.UI.WebControls.TextBox txtPrecio;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvPrecio;
        protected global::System.Web.UI.WebControls.CompareValidator cvPrecio;
        protected global::System.Web.UI.WebControls.Label lblCategoria;
        protected global::System.Web.UI.WebControls.DropDownList ddlCategoria;
        protected global::System.Web.UI.WebControls.Label lblProveedor;
        protected global::System.Web.UI.WebControls.DropDownList ddlProveedor;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvProveedor;
        protected global::System.Web.UI.WebControls.Label lblImagenes;
        protected global::System.Web.UI.WebControls.GridView gvImagenes;
        protected global::System.Web.UI.WebControls.Label lblSinImagenes;
        protected global::System.Web.UI.WebControls.FileUpload fuImagen;
        protected global::System.Web.UI.WebControls.Button btnCargarImagen;
        protected global::System.Web.UI.WebControls.Label lblInfoImagen;
        protected global::System.Web.UI.WebControls.HiddenField hfNuevasImagenes;
        protected global::System.Web.UI.WebControls.HiddenField hfImagenesAEliminar;
        protected global::System.Web.UI.WebControls.Button btnGuardar;
        protected global::System.Web.UI.WebControls.Button btnCancelar;
    }
}
