using System;
using System.Data;
using System.Text.RegularExpressions;
using C_presentacion.Validators;
using FluentValidation;

namespace C_presentacion
{
    public partial class MiPerfil : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                txtFechaNac.Attributes["min"] = FechaMinima.ToString("yyyy-MM-dd");
                txtFechaNac.Attributes["max"] = FechaMaxima.ToString("yyyy-MM-dd");
                CargarDatosUsuario();
            }
        }

        private void CargarDatosUsuario()
        {
            int usuId = Convert.ToInt32(Session["usu_id"]);
            C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();
            DataTable dt = negocio.ObtenerUsuarioPorId(usuId);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtNombre.Text = row["usu_nombre"].ToString();
                txtCorreo.Text = row["usu_correo"].ToString();
                txtCorreoSecundario.Text = row["usu_correo_secundario"].ToString();
                txtCedula.Text = row["usu_cedula"].ToString();
                txtCelular.Text = row["usu_celular"].ToString();

                if (row["usu_fecha_nacimiento"] != DBNull.Value)
                {
                    DateTime fechaNac = Convert.ToDateTime(row["usu_fecha_nacimiento"]);
                    txtFechaNac.Text = fechaNac.ToString("yyyy-MM-dd");

                    if (Session["usu_rol"].ToString() == "Usuario")
                    {
                        MostrarInfoFecha(fechaNac);
                    }
                }
            }
        }

        private void MostrarInfoFecha(DateTime fechaNac)
        {
            pnlInfoFecha.Visible = true;

            lblEdad.Text = "Edad: " + C_negocio.UsuarioNegocio.CalcularEdad(fechaNac);
            lblSigno.Text = C_negocio.UsuarioNegocio.ObtenerSignoZodiacal(fechaNac);
            lblFormatoLargo.Text = fechaNac.ToString("dddd, dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));
            lblFormatoCorto1.Text = "Formato corto: " + fechaNac.ToString("dd/MM/yyyy");
            lblFormatoCorto2.Text = "Formato corto: " + fechaNac.ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("es-ES"));
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int usuId = Convert.ToInt32(Session["usu_id"]);
                C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

                string cedula = txtCedula.Text.Trim();
                DataTable dtActual = negocio.ObtenerUsuarioPorId(usuId);
                string cedulaActual = dtActual.Rows.Count > 0 ? dtActual.Rows[0]["usu_cedula"].ToString() : "";
                if (!string.IsNullOrEmpty(cedula) && cedula != cedulaActual && negocio.CedulaExiste(cedula, usuId))
                {
                    lblMensaje.Text = "La cedula ya esta registrada por otro usuario.";
                    lblMensaje.Visible = true;
                    return;
                }

                DateTime? fechaNac = null;
                if (!string.IsNullOrEmpty(txtFechaNac.Text.Trim()))
                    fechaNac = DateTime.Parse(txtFechaNac.Text.Trim());

                // Construir modelo y validar con FluentValidation
                var modelo = new PerfilViewModel
                {
                    Nombre = txtNombre.Text.Trim(),
                    Cedula = cedula,
                    Celular = txtCelular.Text.Trim(),
                    FechaNacimiento = fechaNac
                };
                var validador = new PerfilValidator();
                var resultado = validador.Validate(modelo);

                if (!resultado.IsValid)
                {
                    // Mostrar la primera validacion que fallo como mensaje
                    lblMensaje.Text = resultado.Errors[0].ErrorMessage;
                    lblMensaje.Visible = true;
                    return;
                }

                string correoSec = txtCorreoSecundario.Text.Trim();
                negocio.ActualizarPerfil(usuId, txtNombre.Text.Trim(), cedula, txtCelular.Text.Trim(), fechaNac, correoSec);

                if (fechaNac.HasValue && Session["usu_rol"].ToString() == "Usuario")
                    MostrarInfoFecha(fechaNac.Value);
                else
                    pnlInfoFecha.Visible = false;

                Session["usu_nombre"] = txtNombre.Text.Trim();

                lblMensaje.Text = "Perfil actualizado exitosamente.";
                lblMensaje.ForeColor = System.Drawing.Color.FromArgb(30, 58, 95);
                if (!string.IsNullOrEmpty(correoSec) && !Regex.IsMatch(correoSec, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    lblMensaje.Text += " Nota: El correo secundario no tiene un formato valido.";
                lblMensaje.Visible = true;
            }
        }
    }
}
