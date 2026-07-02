using System;
using System.Data;

namespace C_presentacion
{
    public partial class Login : System.Web.UI.Page
    {
        // Evento que se ejecuta al hacer clic en el botón "Iniciar Sesión"
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validar los controles del lado del servidor
            if (Page.IsValid)
            {
                C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

                // Verificar si el correo existe en la base de datos
                if (!negocio.CorreoExiste(txtCorreo.Text.Trim()))
                {
                    lblMensaje.Text = "Correo o contraseña incorrectos.";
                    lblMensaje.Visible = true;
                    return;
                }

                // Obtener datos del usuario por correo
                DataTable dtUser = negocio.ObtenerUsuarioPorCorreo(txtCorreo.Text.Trim());
                if (dtUser.Rows.Count == 0) return;

                DataRow row = dtUser.Rows[0];
                int usuId = Convert.ToInt32(row["usu_id"]);
                string estado = row["usu_estado"].ToString();
                int intentos = Convert.ToInt32(row["usu_intentos"]);

                // Verificar si el usuario está bloqueado por intentos fallidos
                if (estado == "B")
                {
                    lblMensaje.Text = "Usuario bloqueado por demasiados intentos fallidos.";
                    lblMensaje.Visible = true;
                    return;
                }

                // Verificar si el usuario está inactivo
                if (estado == "I")
                {
                    lblMensaje.Text = "Usuario inactivo. Contacte al administrador.";
                    lblMensaje.Visible = true;
                    return;
                }

                // Validar la contraseña contra la base de datos
                DataTable dtLogin = negocio.Login(txtCorreo.Text.Trim(), txtContrasena.Text);
                if (dtLogin.Rows.Count > 0)
                {
                    // Inicio de sesión exitoso: guardar datos en la sesión
                    DataRow loginRow = dtLogin.Rows[0];
                    Session["usu_id"] = loginRow["usu_id"];
                    Session["usu_nombre"] = loginRow["usu_nombre"];
                    Session["usu_correo"] = loginRow["usu_correo"];
                    Session["usu_rol"] = loginRow["usu_rol"];

                    // Reiniciar contador de intentos fallidos
                    negocio.ReiniciarIntentos(usuId);

                    // Redirigir a la página principal del sistema
                    Response.Redirect("~/Inicio.aspx");
                }
                else
                {
                    // Contraseña incorrecta: incrementar intentos fallidos
                    negocio.ManejarIntentoFallido(usuId, intentos);

                    lblMensaje.Text = "Correo o contraseña incorrectos.";
                    lblMensaje.Visible = true;
                }
            }
        }
    }
}