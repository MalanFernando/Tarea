using System;
using System.Data;
using System.IO;

namespace C_presentacion
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        // Se ejecuta al cargar la página
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario haya iniciado sesión
            if (Session["usu_id"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarDatosUsuario();
            }

            // Filtrar el diálogo de archivos para que solo muestre imágenes
            fuImagen.Attributes["accept"] = ".jpg,.jpeg,.png,.gif";
            fuImagen.Attributes["onchange"] = "validarImagen(this);";
        }

        // Carga los datos del usuario desde la base de datos
        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
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
                txtCedula.Text = row["usu_cedula"].ToString();
                txtCelular.Text = row["usu_celular"].ToString();

                // Cargar imagen de perfil si existe
                string rutaImagen = row["usu_imagen_path"].ToString();
                if (!string.IsNullOrEmpty(rutaImagen))
                {
                    imgPerfil.ImageUrl = ResolveUrl(rutaImagen);
                    imgPerfil.Visible = true;
                    lblSinImagen.Visible = false;
                }
                else
                {
                    imgPerfil.Visible = false;
                    lblSinImagen.Visible = true;
                }
            }
        }

        // Evento que se ejecuta al hacer clic en "Cargar Imagen"
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            // Verificar que se haya seleccionado un archivo
            if (!fuImagen.HasFile)
            {
                lblMensaje.Text = "Seleccione un archivo de imagen.";
                lblMensaje.Visible = true;
                return;
            }

            // Validar la extensión del archivo
            string extension = Path.GetExtension(fuImagen.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif")
            {
                lblMensaje.Text = "Formato no válido. Use solo imágenes JPG, PNG o GIF.";
                lblMensaje.Visible = true;
                return;
            }

            // Validar el tamaño del archivo (máximo 30 MB)
            if (fuImagen.PostedFile.ContentLength > 31457280)
            {
                lblMensaje.Text = "La imagen es demasiado grande. El tamaño máximo es 30 MB.";
                lblMensaje.Visible = true;
                return;
            }

            // Validar que el archivo sea una imagen real leyendo sus bytes de cabecera
            Stream stream = fuImagen.PostedFile.InputStream;
            if (!EsArchivoImagenValido(stream))
            {
                lblMensaje.Text = "El archivo no es una imagen válida. Solo se permiten JPG, PNG o GIF.";
                lblMensaje.Visible = true;
                return;
            }

            int usuId = Convert.ToInt32(Session["usu_id"]);

            // Eliminar la imagen anterior si existe
            string carpeta = Server.MapPath("~/Imagenes/Perfiles/");
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            // Eliminar imagen anterior del disco si hay una nueva pendiente o ya guardada
            string rutaAnterior = hfNuevaImagen.Value;
            if (!string.IsNullOrEmpty(rutaAnterior))
            {
                string rutaFisicaAnterior = Server.MapPath(rutaAnterior);
                if (File.Exists(rutaFisicaAnterior))
                {
                    File.Delete(rutaFisicaAnterior);
                }
            }

            // Guardar la nueva imagen en el servidor
            string nombreArchivo = $"usu_{usuId}_{Guid.NewGuid()}{extension}";
            string rutaFisica = Path.Combine(carpeta, nombreArchivo);
            fuImagen.SaveAs(rutaFisica);

            // Guardar la ruta relativa en el campo oculto y mostrar la previsualización
            string rutaRelativa = $"~/Imagenes/Perfiles/{nombreArchivo}";
            hfNuevaImagen.Value = rutaRelativa;
            imgPerfil.ImageUrl = ResolveUrl(rutaRelativa);
            imgPerfil.Visible = true;
            lblSinImagen.Visible = false;

            lblMensaje.Text = "Imagen cargada correctamente. Guarde los cambios para confirmar.";
            lblMensaje.ForeColor = System.Drawing.Color.Green;
            lblMensaje.Visible = true;
        }

        // Verifica que los primeros bytes del archivo correspondan a una imagen real
        // (JPG = FF D8 FF, PNG = 89 50 4E 47, GIF = 47 49 46 38)
        private bool EsArchivoImagenValido(Stream stream)
        {
            byte[] cabecera = new byte[4];
            stream.Read(cabecera, 0, 4);
            stream.Position = 0; // Restaurar posición para que SaveAs funcione

            // JPEG: 0xFF 0xD8 0xFF
            if (cabecera[0] == 0xFF && cabecera[1] == 0xD8 && cabecera[2] == 0xFF)
                return true;

            // PNG: 0x89 0x50 0x4E 0x47
            if (cabecera[0] == 0x89 && cabecera[1] == 0x50 && cabecera[2] == 0x4E && cabecera[3] == 0x47)
                return true;

            // GIF: 0x47 0x49 0x46 0x38
            if (cabecera[0] == 0x47 && cabecera[1] == 0x49 && cabecera[2] == 0x46 && cabecera[3] == 0x38)
                return true;

            return false;
        }

        // Evento que se ejecuta al hacer clic en "Guardar Cambios"
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int usuId = Convert.ToInt32(Session["usu_id"]);
                C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

                // Verificar que la cédula (si cambió y no está vacía) no esté ya registrada
                string cedula = txtCedula.Text.Trim();
                DataTable dtActual = negocio.ObtenerUsuarioPorId(usuId);
                string cedulaActual = dtActual.Rows.Count > 0 ? dtActual.Rows[0]["usu_cedula"].ToString() : "";
                if (!string.IsNullOrEmpty(cedula) && cedula != cedulaActual && negocio.CedulaExiste(cedula, usuId))
                {
                    lblMensaje.Text = "La cédula ya está registrada por otro usuario.";
                    lblMensaje.Visible = true;
                    return;
                }

                // Actualizar los datos del perfil (nombre, cedula, celular)
                negocio.ActualizarPerfil(usuId, txtNombre.Text.Trim(), cedula, txtCelular.Text.Trim());

                // Actualizar la imagen de perfil si se cargó una nueva
                if (!string.IsNullOrEmpty(hfNuevaImagen.Value))
                {
                    // Eliminar la imagen anterior del disco (si es diferente a la nueva)
                    DataTable dt = negocio.ObtenerUsuarioPorId(usuId);
                    if (dt.Rows.Count > 0)
                    {
                        string rutaAnterior = dt.Rows[0]["usu_imagen_path"].ToString();
                        if (!string.IsNullOrEmpty(rutaAnterior) && rutaAnterior != hfNuevaImagen.Value)
                        {
                            string rutaFisicaAnterior = Server.MapPath(rutaAnterior);
                            if (File.Exists(rutaFisicaAnterior))
                            {
                                File.Delete(rutaFisicaAnterior);
                            }
                        }
                    }

                    // Guardar la nueva ruta en la base de datos
                    negocio.ActualizarImagenPerfil(usuId, hfNuevaImagen.Value);
                    hfNuevaImagen.Value = "";
                }

                // Actualizar el nombre en la sesión
                Session["usu_nombre"] = txtNombre.Text.Trim();

                lblMensaje.Text = "Perfil actualizado exitosamente.";
                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Visible = true;
            }
        }
    }
}