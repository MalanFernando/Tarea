using System;

namespace C_presentacion
{
    public partial class Register : System.Web.UI.Page
    {
        // Evento que se ejecuta al hacer clic en el botón "Registrarse"
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Validar los controles del lado del servidor
            if (Page.IsValid)
            {
                C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

                // Verificar que el correo no esté ya registrado
                if (negocio.CorreoExiste(txtCorreo.Text.Trim()))
                {
                    lblMensaje.Text = "El correo electrónico ya está registrado.";
                    lblMensaje.Visible = true;
                    return;
                }

                // Registrar al nuevo usuario en la base de datos
                int usuId = negocio.Registrar(
                    txtNombre.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    txtContrasena.Text,
                    txtCedula.Text.Trim(),
                    txtCelular.Text.Trim(),
                    ddlRol.SelectedValue
                );

                if (usuId > 0)
                {
                    // Registro exitoso
                    lblMensaje.Text = "Registro exitoso. Ahora puede iniciar sesión.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    lblMensaje.Visible = true;

                    // Limpiar los campos del formulario
                    txtNombre.Text = "";
                    txtCorreo.Text = "";
                    txtContrasena.Text = "";
                    txtConfirmar.Text = "";
                    txtCedula.Text = "";
                    txtCelular.Text = "";
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