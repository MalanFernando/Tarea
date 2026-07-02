<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="C_presentacion.Register" MasterPageFile="~/MasterPages/Public.Master" ResponseEncoding="utf-8" Title="Registrarse - CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-form">
        <h2 class="auth-title">Registrarse</h2>
        <p class="auth-subtitle">Cree una cuenta para acceder al sistema</p>

        <asp:Label ID="lblMensaje" runat="server" CssClass="message message-error" Visible="false"></asp:Label>

        <div class="field">
            <asp:Label ID="lblNombre" runat="server" CssClass="field-label" Text="Nombre Completo:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="field-input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                CssClass="field-error" EnableClientScript="false"></asp:RequiredFieldValidator>
        </div>

        <div class="field">
            <asp:Label ID="lblCorreo" runat="server" CssClass="field-label" Text="Correo Electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server" CssClass="field-input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                CssClass="field-error" EnableClientScript="false"></asp:RequiredFieldValidator>
        </div>

        <div class="field-inline-group">
            <div class="field">
                <asp:Label ID="lblContrasena" runat="server" CssClass="field-label" Text="Contraseña:"></asp:Label>
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="field-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvContrasena" runat="server"
                    ControlToValidate="txtContrasena" ErrorMessage="La contraseña es obligatoria"
                    CssClass="field-error" EnableClientScript="false"></asp:RequiredFieldValidator>
            </div>
            <div class="field">
                <asp:Label ID="lblConfirmar" runat="server" CssClass="field-label" Text="Confirmar Contraseña:"></asp:Label>
                <asp:TextBox ID="txtConfirmar" runat="server" TextMode="Password" CssClass="field-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server"
                    ControlToValidate="txtConfirmar" ErrorMessage="Confirme la contraseña"
                    CssClass="field-error" EnableClientScript="false"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvContrasenas" runat="server"
                    ControlToValidate="txtConfirmar" ControlToCompare="txtContrasena"
                    ErrorMessage="Las contraseñas no coinciden" CssClass="field-error"
                    EnableClientScript="false"></asp:CompareValidator>
            </div>
        </div>

        <div class="field-inline-group">
            <div class="field">
                <asp:Label ID="lblCedula" runat="server" CssClass="field-label" Text="Cédula (opcional):"></asp:Label>
                <asp:TextBox ID="txtCedula" runat="server" CssClass="field-input"
                    onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revCedula" runat="server"
                    ControlToValidate="txtCedula" ValidationExpression="^\d{0,10}$"
                    ErrorMessage="Solo números (máx. 10 dígitos)" CssClass="field-error"
                    EnableClientScript="false"></asp:RegularExpressionValidator>
            </div>
            <div class="field">
                <asp:Label ID="lblCelular" runat="server" CssClass="field-label" Text="Celular (opcional):"></asp:Label>
                <asp:TextBox ID="txtCelular" runat="server" CssClass="field-input"
                    onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revCelular" runat="server"
                    ControlToValidate="txtCelular" ValidationExpression="^\d{0,10}$"
                    ErrorMessage="Solo números (máx. 10 dígitos)" CssClass="field-error"
                    EnableClientScript="false"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="field">
            <asp:Label ID="lblFechaNac" runat="server" CssClass="field-label" Text="Fecha de Nacimiento:"></asp:Label>
            <asp:TextBox ID="txtFechaNac" runat="server" TextMode="Date" CssClass="field-input"></asp:TextBox>
            <asp:CompareValidator ID="cvFechaNac" runat="server"
                ControlToValidate="txtFechaNac" Operator="DataTypeCheck" Type="Date"
                ErrorMessage="Ingrese una fecha válida" CssClass="field-error"
                EnableClientScript="false"></asp:CompareValidator>
        </div>

        <asp:Button ID="BtnRegistrar" runat="server" Text="Registrarse"
            OnClick="BtnRegistrar_Click" CssClass="btn btn-primary auth-btn" />

        <div class="auth-links">
            <asp:HyperLink ID="hlLogin" runat="server" CssClass="link"
                NavigateUrl="~/Login.aspx">Ya tengo cuenta, iniciar sesión</asp:HyperLink>
        </div>
    </div>
</asp:Content>
