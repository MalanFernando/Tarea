<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recuperar.aspx.cs" Inherits="C_presentacion.Recuperar" MasterPageFile="~/MasterPages/Public.Master" ResponseEncoding="utf-8" Title="Recuperar Contraseña - CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-form">
        <h2 class="auth-title">Recuperar Contraseña</h2>
        <p class="auth-subtitle">Ingrese su correo para recuperar el acceso</p>

        <asp:Label ID="lblMensaje" runat="server" CssClass="message message-error" Visible="false"></asp:Label>

        <div class="field">
            <asp:Label ID="lblCorreo" runat="server" CssClass="field-label" Text="Correo Electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server" CssClass="field-input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                CssClass="field-error" EnableClientScript="false"></asp:RequiredFieldValidator>
        </div>

        <asp:Button ID="BtnRecuperar" runat="server" Text="Recuperar Contraseña"
            OnClick="BtnRecuperar_Click" CssClass="btn btn-primary auth-btn" />

        <asp:Label ID="lblContrasena" runat="server" CssClass="message message-info" Visible="false"></asp:Label>

        <div class="auth-links">
            <asp:HyperLink ID="hlLogin" runat="server" CssClass="link"
                NavigateUrl="~/Login.aspx">Volver al inicio de sesión</asp:HyperLink>
        </div>
    </div>
</asp:Content>
