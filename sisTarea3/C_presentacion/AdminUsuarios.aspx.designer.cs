namespace C_presentacion
{
    public partial class AdminUsuarios
    {
        protected global::System.Web.UI.WebControls.Label lblMensaje;
        protected global::System.Web.UI.WebControls.GridView GvUsuarios;
        protected global::System.Web.UI.WebControls.Button BtnNuevo;
        protected global::System.Web.UI.WebControls.Panel pnlFormulario;
        protected global::System.Web.UI.WebControls.Label lblTituloFormulario;
        protected global::System.Web.UI.WebControls.HiddenField hfUsuarioId;
        protected global::System.Web.UI.WebControls.Label lblNombre;
        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvNombre;
        protected global::System.Web.UI.WebControls.Label lblCorreo;
        protected global::System.Web.UI.WebControls.TextBox txtCorreo;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvCorreo;
        protected global::System.Web.UI.WebControls.Label lblCorreoSecundario;
        protected global::System.Web.UI.WebControls.TextBox txtCorreoSecundario;
        protected global::System.Web.UI.WebControls.Label lblContrasena;
        protected global::System.Web.UI.WebControls.TextBox txtContrasena;
        protected global::System.Web.UI.WebControls.Label lblInfoContrasena;
        protected global::System.Web.UI.WebControls.Label lblConfirmar;
        protected global::System.Web.UI.WebControls.TextBox txtConfirmar;
        protected global::System.Web.UI.WebControls.CompareValidator cvContrasenas;
        protected global::System.Web.UI.WebControls.Label lblCedula;
        protected global::System.Web.UI.WebControls.TextBox txtCedula;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revCedula;
        protected global::System.Web.UI.WebControls.Label lblCelular;
        protected global::System.Web.UI.WebControls.TextBox txtCelular;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revCelular;
        protected global::System.Web.UI.WebControls.Label lblFechaNac;
        protected global::System.Web.UI.WebControls.TextBox txtFechaNac;
        protected global::System.Web.UI.WebControls.CompareValidator cvFechaNac;
        protected global::System.Web.UI.WebControls.Label lblRol;
        protected global::System.Web.UI.WebControls.DropDownList ddlRol;
        protected global::System.Web.UI.WebControls.Button BtnGuardar;
        protected global::System.Web.UI.WebControls.Button BtnCancelar;
    }
}
