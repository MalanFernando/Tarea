using System;

namespace C_presentacion
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usu_id"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}
