using System;

namespace C_presentacion
{
    public partial class Site : System.Web.UI.MasterPage
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
                string nombre = Session["usu_nombre"].ToString();
                lblBienvenida.Text = nombre;
                lblRol.Text = Session["usu_rol"].ToString();
                lblAvatar.Text = nombre.Length > 0 ? nombre[0].ToString().ToUpper() : "U";

                hlAdmin.Visible = (Session["usu_rol"].ToString() == "Admin");
            }
        }

        protected void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
