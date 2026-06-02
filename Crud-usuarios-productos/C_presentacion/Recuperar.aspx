<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recuperar.aspx.cs" Inherits="C_presentacion.Recuperar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Recuperar Contraseña - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/recuperar.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Recuperar Contraseña</h2>

            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <br />

            <asp:Label ID="lblCorreo" runat="server" Text="Ingrese su correo electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            <br /><br />

            <asp:Button ID="btnRecuperar" runat="server" Text="Recuperar Contraseña" OnClick="btnRecuperar_Click" />
            <br /><br />

            <asp:Label ID="lblContrasena" runat="server" Visible="false"></asp:Label>
            <br />

            <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/Login.aspx">Volver al inicio de sesión</asp:HyperLink>
        </div>
    </form>
</body>
</html>
