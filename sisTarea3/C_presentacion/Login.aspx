<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="C_presentacion.Login" MasterPageFile="~/MasterPages/Public.Master" ResponseEncoding="utf-8" Title="Iniciar Sesión - CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-form">
        <h2 class="auth-title">Iniciar Sesión</h2>
        <p class="auth-subtitle">Ingrese sus credenciales para acceder al sistema</p>

        <asp:Label ID="lblMensaje" runat="server" CssClass="message message-error" Visible="false"></asp:Label>

        <div class="field">
            <asp:Label ID="lblCorreo" runat="server" CssClass="field-label" Text="Correo Electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server" CssClass="field-input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                CssClass="field-error" EnableClientScript="false"></asp:RequiredFieldValidator>
        </div>

        <div class="field">
            <asp:Label ID="lblContrasena" runat="server" CssClass="field-label" Text="Contraseña:"></asp:Label>
            <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="field-input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvContrasena" runat="server"
                ControlToValidate="txtContrasena" ErrorMessage="La contraseña es obligatoria"
                CssClass="field-error" EnableClientScript="false"></asp:RequiredFieldValidator>
        </div>

        <asp:Button ID="BtnLogin" runat="server" Text="Iniciar Sesión"
            OnClick="BtnLogin_Click" CssClass="btn btn-primary auth-btn" />

        <div class="auth-links">
            <asp:HyperLink ID="hlRegistro" runat="server" CssClass="link"
                NavigateUrl="~/Register.aspx">Registrarse</asp:HyperLink>
            <asp:HyperLink ID="hlRecuperar" runat="server" CssClass="link"
                NavigateUrl="~/Recuperar.aspx">¿Olvidó su contraseña?</asp:HyperLink>
        </div>
    </div>
</asp:Content>
