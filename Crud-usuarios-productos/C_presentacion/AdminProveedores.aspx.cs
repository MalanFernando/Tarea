using System;
using System.Data;
using System.IO;

namespace C_presentacion
{
    public partial class AdminProveedores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usu_id"] == null || (Session["usu_rol"].ToString() != "Administrador" && Session["usu_rol"].ToString() != "Empleado"))
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
            C_negocio.ProveedorNegocio negocio = new C_negocio.ProveedorNegocio();
            gvProveedores.DataSource = negocio.ListarProveedores();
            gvProveedores.DataBind();
        }

        protected void gvProveedores_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            C_negocio.ProveedorNegocio negocio = new C_negocio.ProveedorNegocio();

            int index = Convert.ToInt32(e.CommandArgument);
            int prvId = Convert.ToInt32(gvProveedores.DataKeys[index]["prv_id"]);

            if (e.CommandName == "Editar")
            {
                DataTable dt = negocio.ObtenerProveedorPorId(prvId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hfProveedorId.Value = prvId.ToString();
                    txtNombre.Text = row["prv_nombre"].ToString();
                    txtContacto.Text = row["prv_contacto"].ToString();
                    txtTelefono.Text = row["prv_telefono"].ToString();
                    txtCorreo.Text = row["prv_correo"].ToString();

                    lblTituloFormulario.Text = "Editar Proveedor";
                    hfNuevaImagen.Value = "";
                    pnlFormulario.Visible = true;

                    string rutaImagen = row["prv_imagen_path"].ToString();
                    if (!string.IsNullOrEmpty(rutaImagen))
                    {
                        imgProveedor.ImageUrl = ResolveUrl(rutaImagen);
                        imgProveedor.Visible = true;
                        lblSinImagen.Visible = false;
                    }
                    else
                    {
                        imgProveedor.Visible = false;
                        lblSinImagen.Visible = true;
                    }
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                if (negocio.TieneProductosAsociados(prvId))
                {
                    lblMensaje.Text = "No se puede eliminar el proveedor porque tiene productos asociados.";
                    lblMensaje.Visible = true;
                    return;
                }

                DataTable dt = negocio.ObtenerProveedorPorId(prvId);
                if (dt.Rows.Count > 0)
                {
                    string rutaImagen = dt.Rows[0]["prv_imagen_path"].ToString();
                    if (!string.IsNullOrEmpty(rutaImagen))
                    {
                        string rutaFisica = Server.MapPath(rutaImagen);
                        if (File.Exists(rutaFisica))
                        {
                            File.Delete(rutaFisica);
                        }
                    }
                }

                negocio.EliminarProveedor(prvId);

                lblMensaje.Text = "Proveedor eliminado correctamente.";
                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Visible = true;
                CargarGrilla();
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblTituloFormulario.Text = "Agregar Proveedor";
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

            int prvId = Convert.ToInt32(hfProveedorId.Value);

            string carpeta = Server.MapPath("~/Imagenes/Proveedores/");
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

            string nombreArchivo = $"prv_{prvId}_{Guid.NewGuid()}{extension}";
            string rutaFisica = Path.Combine(carpeta, nombreArchivo);
            fuImagen.SaveAs(rutaFisica);

            string rutaRelativa = $"~/Imagenes/Proveedores/{nombreArchivo}";
            hfNuevaImagen.Value = rutaRelativa;
            imgProveedor.ImageUrl = ResolveUrl(rutaRelativa);
            imgProveedor.Visible = true;
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
                C_negocio.ProveedorNegocio negocio = new C_negocio.ProveedorNegocio();
                int prvId = Convert.ToInt32(hfProveedorId.Value);

                if (prvId == 0)
                {
                    int nuevoId = negocio.InsertarProveedor(
                        txtNombre.Text.Trim(),
                        txtContacto.Text.Trim(),
                        txtTelefono.Text.Trim(),
                        txtCorreo.Text.Trim()
                    );

                    if (!string.IsNullOrEmpty(hfNuevaImagen.Value))
                    {
                        string carpeta = Server.MapPath("~/Imagenes/Proveedores/");
                        string extension = Path.GetExtension(hfNuevaImagen.Value);
                        string nuevoNombre = $"prv_{nuevoId}_{Guid.NewGuid()}{extension}";
                        string rutaVieja = Server.MapPath(hfNuevaImagen.Value);
                        string rutaNueva = Path.Combine(carpeta, nuevoNombre);
                        if (File.Exists(rutaVieja))
                        {
                            File.Move(rutaVieja, rutaNueva);
                        }
                        string rutaRelativa = $"~/Imagenes/Proveedores/{nuevoNombre}";
                        negocio.ActualizarImagenProveedor(nuevoId, rutaRelativa);
                        hfNuevaImagen.Value = "";
                    }

                    lblMensaje.Text = "Proveedor creado exitosamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    DataTable dt = negocio.ObtenerProveedorPorId(prvId);

                    negocio.ActualizarProveedor(
                        prvId,
                        txtNombre.Text.Trim(),
                        txtContacto.Text.Trim(),
                        txtTelefono.Text.Trim(),
                        txtCorreo.Text.Trim()
                    );

                    if (!string.IsNullOrEmpty(hfNuevaImagen.Value))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            string rutaAnterior = dt.Rows[0]["prv_imagen_path"].ToString();
                            if (!string.IsNullOrEmpty(rutaAnterior) && rutaAnterior != hfNuevaImagen.Value)
                            {
                                string rutaFisicaAnterior = Server.MapPath(rutaAnterior);
                                if (File.Exists(rutaFisicaAnterior))
                                {
                                    File.Delete(rutaFisicaAnterior);
                                }
                            }
                        }

                        negocio.ActualizarImagenProveedor(prvId, hfNuevaImagen.Value);
                        hfNuevaImagen.Value = "";
                    }

                    lblMensaje.Text = "Proveedor actualizado exitosamente.";
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
            hfProveedorId.Value = "0";
            txtNombre.Text = "";
            txtContacto.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            hfNuevaImagen.Value = "";
            imgProveedor.Visible = false;
            lblSinImagen.Visible = false;
        }
    }
}
