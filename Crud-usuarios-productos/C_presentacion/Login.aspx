<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="C_presentacion.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Iniciar Sesión - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/login.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Iniciar Sesión</h2>

            <!-- Mensaje de error o éxito -->
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <br />

            <!-- Campo de correo electrónico -->
            <asp:Label ID="lblCorreo" runat="server" Text="Correo Electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            <br /><br />

            <!-- Campo de contraseña -->
            <asp:Label ID="lblContrasena" runat="server" Text="Contraseña:"></asp:Label>
            <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvContrasena" runat="server"
                ControlToValidate="txtContrasena" ErrorMessage="La contraseña es obligatoria"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            <br /><br />

            <!-- Botón de inicio de sesión -->
            <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" OnClick="btnLogin_Click" />
            <br /><br />

            <!-- Enlaces a registro y recuperación de contraseña -->
            <asp:HyperLink ID="hlRegistro" runat="server" NavigateUrl="~/Register.aspx">Registrarse</asp:HyperLink>
            <br />
            <asp:HyperLink ID="hlRecuperar" runat="server" NavigateUrl="~/Recuperar.aspx">¿Olvidó su contraseña?</asp:HyperLink>
        </div>
    </form>
</body>
</html>