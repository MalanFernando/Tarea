using System;
using System.Data;
using System.IO;

namespace C_presentacion
{
    public partial class AdminUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usu_id"] == null || Session["usu_rol"].ToString() != "Administrador")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarGrilla();
            }

            fuImagen.Attributes["accept"] = ".jpg,.jpeg,.png,.gif";
            fuImagen.Attributes["onchange"] = "validarImagen(this);";
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        private void CargarGrilla()
        {
            C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();
            gvUsuarios.DataSource = negocio.ListarUsuarios();
            gvUsuarios.DataBind();
        }

        protected void gvUsuarios_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();

            int index = Convert.ToInt32(e.CommandArgument);
            int usuId = Convert.ToInt32(gvUsuarios.DataKeys[index]["usu_id"]);

            if (e.CommandName == "Editar")
            {
                DataTable dt = negocio.ObtenerUsuarioPorId(usuId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hfUsuarioId.Value = usuId.ToString();
                    txtNombre.Text = row["usu_nombre"].ToString();
                    txtCorreo.Text = row["usu_correo"].ToString();
                    txtCedula.Text = row["usu_cedula"].ToString();
                    txtCelular.Text = row["usu_celular"].ToString();
                    ddlRol.SelectedValue = row["usu_rol"].ToString();

                    lblTituloFormulario.Text = "Editar Usuario";
                    txtContrasena.Text = "";
                    txtConfirmar.Text = "";
                    hfNuevaImagen.Value = "";
                    pnlFormulario.Visible = true;

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
            else if (e.CommandName == "CambiarEstado")
            {
                DataTable dt = negocio.ObtenerUsuarioPorId(usuId);
                if (dt.Rows.Count > 0)
                {
                    string estadoActual = dt.Rows[0]["usu_estado"].ToString();
                    string nuevoEstado;

                    if (estadoActual == "A")
                        nuevoEstado = "I";
                    else
                        nuevoEstado = "A";

                    negocio.CambiarEstadoUsuario(usuId, nuevoEstado);

                    if (nuevoEstado == "A")
                    {
                        negocio.ReiniciarIntentos(usuId);
                    }

                    lblMensaje.Text = "Estado del usuario actualizado correctamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    lblMensaje.Visible = true;
                    CargarGrilla();
                }
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblTituloFormulario.Text = "Agregar Usuario";
            pnlFormulario.Visible = true;
        }

        protected void btnCargarImagen_Click(object sender, EventArgs e)
        {
            if (!fuImagen.HasFile)
            {
                lblMensaje.Text = "Seleccione un archivo de imagen.";
                lblMensaje.Visible = true;
                return;
            }

            string extension = Path.GetExtension(fuImagen.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif")
            {
                lblMensaje.Text = "Formato no válido. Use solo imágenes JPG, PNG o GIF.";
                lblMensaje.Visible = true;
                return;
            }

            if (fuImagen.PostedFile.ContentLength > 31457280)
            {
                lblMensaje.Text = "La imagen es demasiado grande. El tamaño máximo es 30 MB.";
                lblMensaje.Visible = true;
                return;
            }

            Stream stream = fuImagen.PostedFile.InputStream;
            if (!EsArchivoImagenValido(stream))
            {
                lblMensaje.Text = "El archivo no es una imagen válida. Solo se permiten JPG, PNG o GIF.";
                lblMensaje.Visible = true;
                return;
            }

            int usuId = Convert.ToInt32(hfUsuarioId.Value);

            string carpeta = Server.MapPath("~/Imagenes/Perfiles/");
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            string rutaAnterior = hfNuevaImagen.Value;
            if (!string.IsNullOrEmpty(rutaAnterior))
            {
                string rutaFisicaAnterior = Server.MapPath(rutaAnterior);
                if (File.Exists(rutaFisicaAnterior))
                {
                    File.Delete(rutaFisicaAnterior);
                }
            }

            string nombreArchivo = $"usu_{usuId}_{Guid.NewGuid()}{extension}";
            string rutaFisica = Path.Combine(carpeta, nombreArchivo);
            fuImagen.SaveAs(rutaFisica);

            string rutaRelativa = $"~/Imagenes/Perfiles/{nombreArchivo}";
            hfNuevaImagen.Value = rutaRelativa;
            imgPerfil.ImageUrl = ResolveUrl(rutaRelativa);
            imgPerfil.Visible = true;
            lblSinImagen.Visible = false;

            lblMensaje.Text = "Imagen cargada correctamente. Guarde los cambios para confirmar.";
            lblMensaje.ForeColor = System.Drawing.Color.Green;
            lblMensaje.Visible = true;
        }

        private bool EsArchivoImagenValido(Stream stream)
        {
            byte[] cabecera = new byte[4];
            stream.Read(cabecera, 0, 4);
            stream.Position = 0;

            if (cabecera[0] == 0xFF && cabecera[1] == 0xD8 && cabecera[2] == 0xFF)
                return true;

            if (cabecera[0] == 0x89 && cabecera[1] == 0x50 && cabecera[2] == 0x4E && cabecera[3] == 0x47)
                return true;

            if (cabecera[0] == 0x47 && cabecera[1] == 0x49 && cabecera[2] == 0x46 && cabecera[3] == 0x38)
                return true;

            return false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
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

                    int nuevoId = negocio.Registrar(
                        txtNombre.Text.Trim(),
                        txtCorreo.Text.Trim(),
                        txtContrasena.Text,
                        txtCedula.Text.Trim(),
                        txtCelular.Text.Trim(),
                        ddlRol.SelectedValue
                    );

                    if (!string.IsNullOrEmpty(hfNuevaImagen.Value))
                    {
                        string carpeta = Server.MapPath("~/Imagenes/Perfiles/");
                        string extension = Path.GetExtension(hfNuevaImagen.Value);
                        string nuevoNombre = $"usu_{nuevoId}_{Guid.NewGuid()}{extension}";
                        string rutaVieja = Server.MapPath(hfNuevaImagen.Value);
                        string rutaNueva = Path.Combine(carpeta, nuevoNombre);
                        if (File.Exists(rutaVieja))
                        {
                            File.Move(rutaVieja, rutaNueva);
                        }
                        string rutaRelativa = $"~/Imagenes/Perfiles/{nuevoNombre}";
                        negocio.ActualizarImagenPerfil(nuevoId, rutaRelativa);
                        hfNuevaImagen.Value = "";
                    }

                    lblMensaje.Text = "Usuario creado exitosamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
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

                    negocio.ActualizarUsuario(
                        usuId,
                        txtNombre.Text.Trim(),
                        txtCorreo.Text.Trim(),
                        txtCedula.Text.Trim(),
                        txtCelular.Text.Trim(),
                        ddlRol.SelectedValue
                    );

                    if (!string.IsNullOrEmpty(txtContrasena.Text))
                    {
                        negocio.CambiarContrasena(usuId, txtContrasena.Text);
                    }

                    if (!string.IsNullOrEmpty(hfNuevaImagen.Value))
                    {
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

                        negocio.ActualizarImagenPerfil(usuId, hfNuevaImagen.Value);
                        hfNuevaImagen.Value = "";
                    }

                    lblMensaje.Text = "Usuario actualizado exitosamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                }

                lblMensaje.Visible = true;
                pnlFormulario.Visible = false;
                CargarGrilla();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlFormulario.Visible = false;
            lblMensaje.Visible = false;

            if (!string.IsNullOrEmpty(hfNuevaImagen.Value))
            {
                string rutaFisica = Server.MapPath(hfNuevaImagen.Value);
                if (File.Exists(rutaFisica))
                {
                    File.Delete(rutaFisica);
                }
                hfNuevaImagen.Value = "";
            }
        }

        private void LimpiarFormulario()
        {
            hfUsuarioId.Value = "0";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtContrasena.Text = "";
            txtConfirmar.Text = "";
            txtCedula.Text = "";
            txtCelular.Text = "";
            ddlRol.SelectedIndex = 0;
            hfNuevaImagen.Value = "";
            imgPerfil.Visible = false;
            lblSinImagen.Visible = false;
        }
    }
}
