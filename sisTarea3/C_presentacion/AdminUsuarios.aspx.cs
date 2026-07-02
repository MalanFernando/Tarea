using System;
using System.Data;
using System.Text.RegularExpressions;

namespace C_presentacion
{
    public partial class AdminUsuarios : System.Web.UI.Page
    {
        private static DateTime FechaMinima => DateTime.Today.AddYears(-100);
        private static DateTime FechaMaxima => DateTime.Today.AddYears(-12);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usu_id"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (Session["usu_rol"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        private void CargarGrilla()
        {
            C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();
            GvUsuarios.DataSource = negocio.ListarUsuarios();
            GvUsuarios.DataBind();
        }

        protected void GvUsuarios_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

            int index = Convert.ToInt32(e.CommandArgument);
            int usuId = Convert.ToInt32(GvUsuarios.DataKeys[index]["usu_id"]);

            if (e.CommandName == "Editar")
            {
                DataTable dt = negocio.ObtenerUsuarioPorId(usuId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hfUsuarioId.Value = usuId.ToString();
                    txtNombre.Text = row["usu_nombre"].ToString();
                    txtCorreo.Text = row["usu_correo"].ToString();
                    txtCorreoSecundario.Text = row["usu_correo_secundario"].ToString();
                    txtCedula.Text = row["usu_cedula"].ToString();
                    txtCelular.Text = row["usu_celular"].ToString();
                    if (row["usu_fecha_nacimiento"] != DBNull.Value)
                        txtFechaNac.Text = Convert.ToDateTime(row["usu_fecha_nacimiento"]).ToString("yyyy-MM-dd");
                    else
                        txtFechaNac.Text = "";
                    ddlRol.SelectedValue = row["usu_rol"].ToString();

                    lblTituloFormulario.Text = "Editar Usuario";
                    txtContrasena.Text = "";
                    txtConfirmar.Text = "";
                    pnlFormulario.Visible = true;
                }
            }
            else if (e.CommandName == "CambiarEstado")
            {
                DataTable dt = negocio.ObtenerUsuarioPorId(usuId);
                if (dt.Rows.Count > 0)
                {
                    string estadoActual = dt.Rows[0]["usu_estado"].ToString();
                    string nuevoEstado = (estadoActual == "A") ? "I" : "A";
                    negocio.CambiarEstadoUsuario(usuId, nuevoEstado);

                    if (nuevoEstado == "A")
                        negocio.ReiniciarIntentos(usuId);

                    lblMensaje.Text = "Estado del usuario actualizado correctamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.FromArgb(30, 58, 95);
                    lblMensaje.Visible = true;
                    CargarGrilla();
                }
            }
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblTituloFormulario.Text = "Agregar Usuario";
            pnlFormulario.Visible = true;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();
                int usuId = Convert.ToInt32(hfUsuarioId.Value);

                if (usuId == 0)
                {
                    if (string.IsNullOrEmpty(txtContrasena.Text))
                    {
                        lblMensaje.Text = "La contraseña es obligatoria para un nuevo usuario.";
                        lblMensaje.Visible = true;
                        return;
                    }

                    if (negocio.CorreoExiste(txtCorreo.Text.Trim()))
                    {
                        lblMensaje.Text = "El correo electrónico ya está registrado.";
                        lblMensaje.Visible = true;
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtCedula.Text.Trim()) && negocio.CedulaExiste(txtCedula.Text.Trim()))
                    {
                        lblMensaje.Text = "La cédula ya está registrada por otro usuario.";
                        lblMensaje.Visible = true;
                        return;
                    }

                    DateTime? fechaNac = null;
                    if (!string.IsNullOrEmpty(txtFechaNac.Text.Trim()))
                        fechaNac = DateTime.Parse(txtFechaNac.Text.Trim());

                    if (fechaNac.HasValue && (fechaNac.Value < FechaMinima || fechaNac.Value > FechaMaxima))
                    {
                        lblMensaje.Text = "La fecha de nacimiento debe corresponder a una edad entre 12 y 100 años.";
                        lblMensaje.Visible = true;
                        return;
                    }

                    string correoSec = txtCorreoSecundario.Text.Trim();
                    int nuevoId = negocio.Registrar(
                        txtNombre.Text.Trim(),
                        txtCorreo.Text.Trim(),
                        txtContrasena.Text,
                        txtCedula.Text.Trim(),
                        txtCelular.Text.Trim(),
                        ddlRol.SelectedValue,
                        fechaNac,
                        correoSec
                    );

                    lblMensaje.Text = "Usuario creado exitosamente.";
                    if (!string.IsNullOrEmpty(correoSec) && !Regex.IsMatch(correoSec, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                        lblMensaje.Text += " Nota: El correo secundario no tiene un formato válido.";
                    lblMensaje.ForeColor = System.Drawing.Color.FromArgb(30, 58, 95);
                }
                else
                {
                    DataTable dt = negocio.ObtenerUsuarioPorId(usuId);
                    string correoActual = dt.Rows[0]["usu_correo"].ToString();
                    string cedulaActual = dt.Rows[0]["usu_cedula"].ToString();

                    if (txtCorreo.Text.Trim() != correoActual && negocio.CorreoExiste(txtCorreo.Text.Trim()))
                    {
                        lblMensaje.Text = "El correo electrónico ya está registrado por otro usuario.";
                        lblMensaje.Visible = true;
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtCedula.Text.Trim()) && txtCedula.Text.Trim() != cedulaActual && negocio.CedulaExiste(txtCedula.Text.Trim(), usuId))
                    {
                        lblMensaje.Text = "La cédula ya está registrada por otro usuario.";
                        lblMensaje.Visible = true;
                        return;
                    }

                    DateTime? fechaNacEdit = null;
                    if (!string.IsNullOrEmpty(txtFechaNac.Text.Trim()))
                        fechaNacEdit = DateTime.Parse(txtFechaNac.Text.Trim());

                    if (fechaNacEdit.HasValue && (fechaNacEdit.Value < FechaMinima || fechaNacEdit.Value > FechaMaxima))
                    {
                        lblMensaje.Text = "La fecha de nacimiento debe corresponder a una edad entre 12 y 100 años.";
                        lblMensaje.Visible = true;
                        return;
                    }

                    string correoSec = txtCorreoSecundario.Text.Trim();
                    negocio.ActualizarUsuario(
                        usuId,
                        txtNombre.Text.Trim(),
                        txtCorreo.Text.Trim(),
                        txtCedula.Text.Trim(),
                        txtCelular.Text.Trim(),
                        ddlRol.SelectedValue,
                        fechaNacEdit,
                        correoSec
                    );

                    if (!string.IsNullOrEmpty(txtContrasena.Text))
                        negocio.CambiarContrasena(usuId, txtContrasena.Text);

                    lblMensaje.Text = "Usuario actualizado exitosamente.";
                    if (!string.IsNullOrEmpty(correoSec) && !Regex.IsMatch(correoSec, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                        lblMensaje.Text += " Nota: El correo secundario no tiene un formato válido.";
                    lblMensaje.ForeColor = System.Drawing.Color.FromArgb(30, 58, 95);
                }

                lblMensaje.Visible = true;
                pnlFormulario.Visible = false;
                CargarGrilla();
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            pnlFormulario.Visible = false;
            lblMensaje.Visible = false;
        }

        private void LimpiarFormulario()
        {
            hfUsuarioId.Value = "0";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtCorreoSecundario.Text = "";
            txtContrasena.Text = "";
            txtConfirmar.Text = "";
            txtCedula.Text = "";
            txtCelular.Text = "";
            txtFechaNac.Text = "";
            ddlRol.SelectedIndex = 0;
        }
    }
}
