<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="C_presentacion.MiPerfil" MasterPageFile="~/MasterPages/Site.Master" ResponseEncoding="utf-8" Title="Mi Perfil — CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-header">
            <h2>Mi Perfil</h2>
        </div>
        <div class="card-body">

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
        <asp:TextBox ID="txtCorreo" runat="server" ReadOnly="true" CssClass="field-input"></asp:TextBox>
    </div>

    <div class="field">
        <asp:Label ID="lblCorreoSecundario" runat="server" CssClass="field-label" Text="Correo Secundario:"></asp:Label>
        <asp:TextBox ID="txtCorreoSecundario" runat="server" CssClass="field-input" onkeyup="valCorreoSec(this)"></asp:TextBox>
        <span id="spnCorreoSec" class="correo-sec-hint"></span>
    </div>

    <div class="field-inline-group">
        <div class="field">
            <asp:Label ID="lblCedula" runat="server" CssClass="field-label" Text="Cédula:"></asp:Label>
            <asp:TextBox ID="txtCedula" runat="server" CssClass="field-input"
                onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revCedula" runat="server"
                ControlToValidate="txtCedula" ValidationExpression="^\d*$"
                ErrorMessage="Solo se permiten números" CssClass="field-error"
                EnableClientScript="false"></asp:RegularExpressionValidator>
        </div>
        <div class="field">
            <asp:Label ID="lblCelular" runat="server" CssClass="field-label" Text="Celular:"></asp:Label>
            <asp:TextBox ID="txtCelular" runat="server" CssClass="field-input"
                onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revCelular" runat="server"
                ControlToValidate="txtCelular" ValidationExpression="^\d*$"
                ErrorMessage="Solo números" CssClass="field-error"
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

    <asp:Panel ID="pnlInfoFecha" runat="server" Visible="false" CssClass="info-panel">
        <div class="info-panel-row">
            <asp:Label ID="lblEdad" runat="server" CssClass="info-age"></asp:Label>
        </div>
        <div class="info-panel-row">
            <asp:Label ID="lblSigno" runat="server" CssClass="info-sign"></asp:Label>
        </div>
        <div class="info-panel-row">
            <asp:Label ID="lblFormatoLargo" runat="server" CssClass="info-format"></asp:Label>
        </div>
        <div class="info-panel-row">
            <asp:Label ID="lblFormatoCorto1" runat="server" CssClass="info-format"></asp:Label>
        </div>
        <div class="info-panel-row">
            <asp:Label ID="lblFormatoCorto2" runat="server" CssClass="info-format"></asp:Label>
        </div>
    </asp:Panel>

    <asp:Button ID="BtnGuardar" runat="server" Text="Guardar Cambios"
        OnClick="BtnGuardar_Click" CssClass="btn btn-primary" />

        </div>
    </div>

<script>
function valCorreoSec(el) {
    var s = document.getElementById('spnCorreoSec');
    if (el.value == '') { s.textContent = ''; return; }
    var re = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (re.test(el.value)) { s.textContent = '✓ Correo válido'; s.style.color = 'green'; }
    else { s.textContent = '✗ Correo inválido (solo ASCII, sin ñ/tildes)'; s.style.color = 'red'; }
}
</script>
</asp:Content>
