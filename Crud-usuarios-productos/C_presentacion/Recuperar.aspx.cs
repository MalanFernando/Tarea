using System;

namespace C_presentacion
{
    public partial class Recuperar : System.Web.UI.Page
    {
        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

                if (!negocio.CorreoExiste(txtCorreo.Text.Trim()))
                {
                    lblMensaje.Text = "El correo ingresado no está registrado.";
                    lblMensaje.Visible = true;
                    lblContrasena.Visible = false;
                    return;
                }

                string contrasena = negocio.RecuperarContrasena(txtCorreo.Text.Trim());

                if (!string.IsNullOrEmpty(contrasena))
                {
                    lblContrasena.Text = "Su contraseña es: " + contrasena;
                    lblContrasena.ForeColor = System.Drawing.Color.Green;
                    lblContrasena.Visible = true;
                    lblMensaje.Visible = false;
                }
                else
                {
                    lblMensaje.Text = "Error al recuperar la contraseña.";
                    lblMensaje.Visible = true;
                    lblContrasena.Visible = false;
                }
            }
        }
    }
}
