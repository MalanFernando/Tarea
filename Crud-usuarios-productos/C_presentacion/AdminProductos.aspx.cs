using System;
using System.Data;
using System.IO;

namespace C_presentacion
{
    public partial class AdminProductos : System.Web.UI.Page
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

        private void CargarGrilla()
        {
            C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();
            gvProductos.DataSource = negocio.ListarProductos();
            gvProductos.DataBind();
        }

        private void CargarCombos()
        {
            C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();
            C_negocio.ProveedorNegocio provNegocio = new C_negocio.ProveedorNegocio();

            ddlCategoria.DataSource = negocio.ListarCategorias();
            ddlCategoria.DataTextField = "cat_nombre";
            ddlCategoria.DataValueField = "cat_id";
            ddlCategoria.DataBind();

            DataTable dtProveedores = provNegocio.ListarProveedores();
            ddlProveedor.DataSource = dtProveedores;
            ddlProveedor.DataTextField = "prv_nombre";
            ddlProveedor.DataValueField = "prv_id";
            ddlProveedor.DataBind();
            ddlProveedor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione un proveedor --", ""));
        }

        protected void gvProductos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();

            int index = Convert.ToInt32(e.CommandArgument);
            int prId = Convert.ToInt32(gvProductos.DataKeys[index]["pr_id"]);

            if (e.CommandName == "Editar")
            {
                DataTable dt = negocio.ObtenerProductoPorId(prId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hfProductoId.Value = prId.ToString();
                    txtNombre.Text = row["pr_nombre"].ToString();
                    txtDescripcion.Text = row["pr_descripcion"].ToString();
                    txtPrecio.Text = row["pr_precio"].ToString();
                    hfNuevasImagenes.Value = "";
                    hfImagenesAEliminar.Value = "";

                    lblTituloFormulario.Text = "Editar Producto";
                    pnlFormulario.Visible = true;

                    CargarCombos();
                    ddlCategoria.SelectedValue = row["pr_categoria_id"].ToString();
                    if (row["pr_proveedor_id"] != DBNull.Value)
                        ddlProveedor.SelectedValue = row["pr_proveedor_id"].ToString();
                    else
                        ddlProveedor.SelectedIndex = 0;

                    RefrescarGridImagenes();
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                DataTable dtImg = negocio.ListarImagenesPorProducto(prId);
                foreach (DataRow imgRow in dtImg.Rows)
                {
                    string ruta = imgRow["pim_path"].ToString();
                    if (!string.IsNullOrEmpty(ruta))
                    {
                        string rutaFisica = Server.MapPath(ruta);
                        if (File.Exists(rutaFisica))
                            File.Delete(rutaFisica);
                    }
                }

                negocio.EliminarProducto(prId);

                lblMensaje.Text = "Producto eliminado correctamente.";
                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Visible = true;
                CargarGrilla();
            }
        }

        protected void gvImagenes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string imgId = gvImagenes.DataKeys[index]["img_id"].ToString();

                if (imgId.StartsWith("E_"))
                {
                    string pimId = imgId.Substring(2);
                    if (string.IsNullOrEmpty(hfImagenesAEliminar.Value))
                        hfImagenesAEliminar.Value = pimId;
                    else
                        hfImagenesAEliminar.Value += "," + pimId;
                }
                else if (imgId.StartsWith("N_"))
                {
                    string path = imgId.Substring(2);
                    string rutaFisica = Server.MapPath(path);
                    if (File.Exists(rutaFisica))
                        File.Delete(rutaFisica);

                    string[] nuevas = hfNuevasImagenes.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    System.Collections.Generic.List<string> lista = new System.Collections.Generic.List<string>(nuevas);
                    lista.Remove(path);
                    hfNuevasImagenes.Value = string.Join(",", lista.ToArray());
                }

                RefrescarGridImagenes();
            }
        }

        private void RefrescarGridImagenes()
        {
            int prId = Convert.ToInt32(hfProductoId.Value);
            DataTable dt = new DataTable();
            dt.Columns.Add("img_id", typeof(string));
            dt.Columns.Add("img_ruta", typeof(string));

            if (prId > 0)
            {
                C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();
                DataTable dtImg = negocio.ListarImagenesPorProducto(prId);
                string[] aEliminar = hfImagenesAEliminar.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (DataRow row in dtImg.Rows)
                {
                    string pimId = row["pim_id"].ToString();
                    bool marcadoEliminar = false;
                    foreach (string id in aEliminar)
                    {
                        if (id == pimId) { marcadoEliminar = true; break; }
                    }
                    if (marcadoEliminar) continue;

                    dt.Rows.Add("E_" + pimId, ResolveUrl(row["pim_path"].ToString()));
                }
            }

            if (!string.IsNullOrEmpty(hfNuevasImagenes.Value))
            {
                string[] nuevas = hfNuevasImagenes.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string path in nuevas)
                {
                    dt.Rows.Add("N_" + path, ResolveUrl(path));
                }
            }

            gvImagenes.DataSource = dt;
            gvImagenes.DataBind();

            lblSinImagenes.Visible = (dt.Rows.Count == 0);
            gvImagenes.Visible = (dt.Rows.Count > 0);

            btnCargarImagen.Enabled = (dt.Rows.Count < 3);
            fuImagen.Enabled = (dt.Rows.Count < 3);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblTituloFormulario.Text = "Agregar Producto";
            pnlFormulario.Visible = true;
            CargarCombos();
            RefrescarGridImagenes();
        }

        protected void btnCargarImagen_Click(object sender, EventArgs e)
        {
            if (!fuImagen.HasFile)
            {
                lblMensaje.Text = "Seleccione un archivo de imagen.";
                lblMensaje.Visible = true;
                return;
            }

            int totalActual = ObtenerTotalImagenesActuales();
            if (totalActual >= 3)
            {
                lblMensaje.Text = "Máximo 3 imágenes por producto.";
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

            string carpeta = Server.MapPath("~/Imagenes/Productos/");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string nombreArchivo = $"temp_{Guid.NewGuid()}{extension}";
            string rutaFisica = Path.Combine(carpeta, nombreArchivo);
            fuImagen.SaveAs(rutaFisica);

            string rutaRelativa = $"~/Imagenes/Productos/{nombreArchivo}";

            if (string.IsNullOrEmpty(hfNuevasImagenes.Value))
                hfNuevasImagenes.Value = rutaRelativa;
            else
                hfNuevasImagenes.Value += "," + rutaRelativa;

            RefrescarGridImagenes();

            lblMensaje.Text = "Imagen agregada correctamente. Guarde los cambios para confirmar.";
            lblMensaje.ForeColor = System.Drawing.Color.Green;
            lblMensaje.Visible = true;
        }

        private int ObtenerTotalImagenesActuales()
        {
            int count = 0;
            int prId = Convert.ToInt32(hfProductoId.Value);

            if (prId > 0)
            {
                C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();
                DataTable dt = negocio.ListarImagenesPorProducto(prId);
                string[] aEliminar = hfImagenesAEliminar.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (DataRow row in dt.Rows)
                {
                    bool marcado = false;
                    foreach (string id in aEliminar)
                    {
                        if (id == row["pim_id"].ToString()) { marcado = true; break; }
                    }
                    if (!marcado) count++;
                }
            }

            if (!string.IsNullOrEmpty(hfNuevasImagenes.Value))
            {
                string[] nuevas = hfNuevasImagenes.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                count += nuevas.Length;
            }

            return count;
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
                C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();
                int prId = Convert.ToInt32(hfProductoId.Value);
                decimal precio;
                if (!decimal.TryParse(txtPrecio.Text.Trim(), out precio))
                {
                    lblMensaje.Text = "Ingrese un precio válido.";
                    lblMensaje.Visible = true;
                    return;
                }
                int categoriaId = Convert.ToInt32(ddlCategoria.SelectedValue);
                int proveedorId = Convert.ToInt32(ddlProveedor.SelectedValue);

                if (negocio.ProductoExiste(txtNombre.Text.Trim(), prId == 0 ? (int?)null : prId))
                {
                    lblMensaje.Text = "Ya existe un producto con ese nombre.";
                    lblMensaje.Visible = true;
                    return;
                }

                if (prId == 0)
                {
                    int usuId = Convert.ToInt32(Session["usu_id"]);
                    int nuevoId = negocio.InsertarProducto(
                        txtNombre.Text.Trim(),
                        txtDescripcion.Text.Trim(),
                        precio,
                        categoriaId,
                        usuId,
                        proveedorId
                    );

                    if (!string.IsNullOrEmpty(hfNuevasImagenes.Value))
                    {
                        string carpeta = Server.MapPath("~/Imagenes/Productos/");
                        string[] nuevas = hfNuevasImagenes.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < nuevas.Length; i++)
                        {
                            string extension = Path.GetExtension(nuevas[i]);
                            string nuevoNombre = $"pr_{nuevoId}_{Guid.NewGuid()}{extension}";
                            string rutaVieja = Server.MapPath(nuevas[i]);
                            string rutaNueva = Path.Combine(carpeta, nuevoNombre);
                            if (File.Exists(rutaVieja))
                                File.Move(rutaVieja, rutaNueva);
                            string rutaRelativa = $"~/Imagenes/Productos/{nuevoNombre}";
                            negocio.InsertarImagenProducto(nuevoId, rutaRelativa, i + 1);
                        }
                        hfNuevasImagenes.Value = "";
                    }

                    lblMensaje.Text = "Producto creado exitosamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    negocio.ActualizarProducto(prId,
                        txtNombre.Text.Trim(),
                        txtDescripcion.Text.Trim(),
                        precio,
                        categoriaId,
                        proveedorId
                    );

                    if (!string.IsNullOrEmpty(hfImagenesAEliminar.Value))
                    {
                        string[] aEliminar = hfImagenesAEliminar.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string idStr in aEliminar)
                        {
                            DataTable dtImg = negocio.ListarImagenesPorProducto(prId);
                            foreach (DataRow row in dtImg.Rows)
                            {
                                if (row["pim_id"].ToString() == idStr)
                                {
                                    string ruta = row["pim_path"].ToString();
                                    if (!string.IsNullOrEmpty(ruta))
                                    {
                                        string rutaFisica = Server.MapPath(ruta);
                                        if (File.Exists(rutaFisica))
                                            File.Delete(rutaFisica);
                                    }
                                    break;
                                }
                            }
                            negocio.EliminarImagenProducto(Convert.ToInt32(idStr));
                        }
                        hfImagenesAEliminar.Value = "";
                    }

                    if (!string.IsNullOrEmpty(hfNuevasImagenes.Value))
                    {
                        string carpeta = Server.MapPath("~/Imagenes/Productos/");
                        string[] nuevas = hfNuevasImagenes.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        DataTable dtImgActual = negocio.ListarImagenesPorProducto(prId);
                        int ordenActual = dtImgActual.Rows.Count + 1;

                        for (int i = 0; i < nuevas.Length; i++)
                        {
                            string extension = Path.GetExtension(nuevas[i]);
                            string nuevoNombre = $"pr_{prId}_{Guid.NewGuid()}{extension}";
                            string rutaVieja = Server.MapPath(nuevas[i]);
                            string rutaNueva = Path.Combine(carpeta, nuevoNombre);
                            if (File.Exists(rutaVieja))
                                File.Move(rutaVieja, rutaNueva);
                            string rutaRelativa = $"~/Imagenes/Productos/{nuevoNombre}";
                            negocio.InsertarImagenProducto(prId, rutaRelativa, ordenActual + i);
                        }
                        hfNuevasImagenes.Value = "";
                    }

                    lblMensaje.Text = "Producto actualizado exitosamente.";
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

            if (!string.IsNullOrEmpty(hfNuevasImagenes.Value))
            {
                string[] nuevas = hfNuevasImagenes.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string path in nuevas)
                {
                    string rutaFisica = Server.MapPath(path);
                    if (File.Exists(rutaFisica))
                        File.Delete(rutaFisica);
                }
                hfNuevasImagenes.Value = "";
            }
        }

        private void LimpiarFormulario()
        {
            hfProductoId.Value = "0";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecio.Text = "";
            hfNuevasImagenes.Value = "";
            hfImagenesAEliminar.Value = "";
            lblSinImagenes.Visible = true;
            gvImagenes.Visible = false;
            btnCargarImagen.Enabled = true;
            fuImagen.Enabled = true;
        }
    }
}
