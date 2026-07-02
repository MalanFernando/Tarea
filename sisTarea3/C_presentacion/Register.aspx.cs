using System;

namespace C_presentacion
{
    public partial class Register : System.Web.UI.Page
    {
        private static DateTime FechaMinima => DateTime.Today.AddYears(-100);
        private static DateTime FechaMaxima => DateTime.Today.AddYears(-12);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaNac.Attributes["min"] = FechaMinima.ToString("yyyy-MM-dd");
                txtFechaNac.Attributes["max"] = FechaMaxima.ToString("yyyy-MM-dd");
            }
        }

        protected void BtnRegistrar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

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

                int usuId = negocio.Registrar(
                    txtNombre.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    txtContrasena.Text,
                    txtCedula.Text.Trim(),
                    txtCelular.Text.Trim(),
                    "Usuario",
                    fechaNac
                );

                if (usuId > 0)
                {
                    lblMensaje.Text = "Registro exitoso. Ahora puede iniciar sesión.";
                    lblMensaje.ForeColor = System.Drawing.Color.FromArgb(30, 58, 95);
                    lblMensaje.Visible = true;

                    txtNombre.Text = "";
                    txtCorreo.Text = "";
                    txtContrasena.Text = "";
                    txtConfirmar.Text = "";
                    txtCedula.Text = "";
                    txtCelular.Text = "";
                    txtFechaNac.Text = "";
                }
                else
                {
                    lblMensaje.Text = "Error al registrar el usuario. Intente nuevamente.";
                    lblMensaje.Visible = true;
                }
            }
        }
    }
}
