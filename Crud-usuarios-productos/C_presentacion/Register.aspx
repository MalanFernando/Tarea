<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="C_presentacion.Register" ResponseEncoding="utf-8" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Registrarse - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/register.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Registrarse</h2>

            <!-- Mensaje de error o éxito -->
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            

            <!-- Campo de nombre completo -->
            <asp:Label ID="lblNombre" runat="server" Text="Nombre Completo:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            

            <!-- Campo de correo electrónico -->
            <asp:Label ID="lblCorreo" runat="server" Text="Correo Electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            

            <!-- Campo de contraseña -->
            <asp:Label ID="lblContrasena" runat="server" Text="Contraseña:"></asp:Label>
            <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvContrasena" runat="server"
                ControlToValidate="txtContrasena" ErrorMessage="La contraseña es obligatoria"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            

            <!-- Campo de confirmar contraseña -->
            <asp:Label ID="lblConfirmar" runat="server" Text="Confirmar Contraseña:"></asp:Label>
            <asp:TextBox ID="txtConfirmar" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server"
                ControlToValidate="txtConfirmar" ErrorMessage="Confirme la contraseña"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvContrasenas" runat="server"
                ControlToValidate="txtConfirmar" ControlToCompare="txtContrasena"
                ErrorMessage="Las contraseñas no coinciden" ForeColor="Red"
                EnableClientScript="false"></asp:CompareValidator>
            

            <!-- Campo de cédula (opcional) -->
            <asp:Label ID="lblCedula" runat="server" Text="Cédula (opcional):"></asp:Label>
            <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
            

            <!-- Campo de celular (opcional) -->
            <asp:Label ID="lblCelular" runat="server" Text="Celular (opcional):"></asp:Label>
            <asp:TextBox ID="txtCelular" runat="server"></asp:TextBox>
            

            <!-- Selección de rol -->
            <asp:Label ID="lblRol" runat="server" Text="Rol:"></asp:Label>
            <asp:DropDownList ID="ddlRol" runat="server">
                <asp:ListItem Text="Empleado" Value="Empleado"></asp:ListItem>
                <asp:ListItem Text="Administrador" Value="Administrador"></asp:ListItem>
            </asp:DropDownList>
            

            <!-- Botón de registro -->
            <asp:Button ID="btnRegistrar" runat="server" Text="Registrarse" OnClick="btnRegistrar_Click" />
            

            <!-- Enlace para volver al login -->
            <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/Login.aspx">Ya tengo cuenta, iniciar sesión</asp:HyperLink>
        </div>
    </form>
</body>
</html>