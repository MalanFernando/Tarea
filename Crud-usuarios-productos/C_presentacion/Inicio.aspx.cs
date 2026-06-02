using System;
using System.Data;
using System.Web.Script.Serialization;

namespace C_presentacion
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usu_id"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblBienvenida.Text = "Bienvenido, " + Session["usu_nombre"].ToString();
                lblRol.Text = "Rol: " + Session["usu_rol"].ToString();

                if (Session["usu_rol"].ToString() == "Administrador")
                {
                    hlAdmin.Visible = true;
                    pnlCharts.Visible = true;
                }

                if (Session["usu_rol"].ToString() == "Administrador" || Session["usu_rol"].ToString() == "Empleado")
                {
                    hlProveedores.Visible = true;
                    hlProductos.Visible = true;
                }
            }
        }

        protected string GetProductosPorCategoriaJSON()
        {
            C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();
            DataTable dt = negocio.ObtenerProductosPorCategoria();
            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();
            foreach (DataRow row in dt.Rows)
            {
                labels.Add(row["nombre"].ToString());
                data.Add(Convert.ToInt32(row["total"]));
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(new { labels = labels, data = data });
        }

        protected string GetUsuariosPorRolJSON()
        {
            C_negocio.UsuarioNegocio negocio = new C_negocio.UsuarioNegocio();
            DataTable dt = negocio.ObtenerUsuariosPorRol();
            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();
            foreach (DataRow row in dt.Rows)
            {
                labels.Add(row["nombre"].ToString());
                data.Add(Convert.ToInt32(row["total"]));
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(new { labels = labels, data = data });
        }

        protected string GetProductosPorProveedorJSON()
        {
            C_negocio.ProductoNegocio negocio = new C_negocio.ProductoNegocio();
            DataTable dt = negocio.ObtenerProductosPorProveedor();
            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();
            foreach (DataRow row in dt.Rows)
            {
                labels.Add(row["nombre"].ToString());
                data.Add(Convert.ToInt32(row["total"]));
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(new { labels = labels, data = data });
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}