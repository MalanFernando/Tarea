<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs" Inherits="C_presentacion.AdminUsuarios" MasterPageFile="~/MasterPages/Site.Master" ResponseEncoding="utf-8" Title="Usuarios — CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-header">
            <h2>Usuarios</h2>
        </div>
        <div class="card-body">

    <asp:Label ID="lblMensaje" runat="server" CssClass="message message-error" Visible="false"></asp:Label>

    <div class="toolbar">
        <div class="toolbar-left"></div>
        <div class="toolbar-right">
            <asp:Button ID="BtnNuevo" runat="server" Text="Agregar Usuario"
                OnClick="BtnNuevo_Click" CssClass="btn btn-primary btn-sm" />
        </div>
    </div>

    <div class="table-wrapper">
    <asp:GridView ID="GvUsuarios" runat="server" AutoGenerateColumns="false"
        OnRowCommand="GvUsuarios_RowCommand" CellPadding="5" DataKeyNames="usu_id"
        CssClass="data-table" GridLines="None" ShowHeaderWhenEmpty="true">
        <HeaderStyle HorizontalAlign="Left" />
        <Columns>
            <asp:BoundField DataField="usu_id" HeaderText="ID" />
            <asp:BoundField DataField="usu_nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="usu_correo" HeaderText="Correo" />
            <asp:BoundField DataField="usu_correo_secundario" HeaderText="Correo Sec." />
            <asp:BoundField DataField="usu_cedula" HeaderText="Cédula" />
            <asp:BoundField DataField="usu_celular" HeaderText="Celular" />
            <asp:BoundField DataField="usu_fecha_nacimiento" HeaderText="Fecha Nac." DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="usu_rol" HeaderText="Rol" />
            <asp:BoundField DataField="usu_estado" HeaderText="Estado" />
            <asp:BoundField DataField="usu_intentos" HeaderText="Intentos" />
            <asp:ButtonField Text="Editar" CommandName="Editar" ButtonType="Button" />
            <asp:ButtonField Text="Activar/Desactivar" CommandName="CambiarEstado" ButtonType="Button" />
        </Columns>
    </asp:GridView>
    </div>

    <asp:Panel ID="pnlFormulario" runat="server" Visible="false" CssClass="form-panel">
        <h3><asp:Label ID="lblTituloFormulario" runat="server" Text="Agregar Usuario"></asp:Label></h3>

        <asp:HiddenField ID="hfUsuarioId" runat="server" Value="0" />

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

        <div class="field">
            <asp:Label ID="lblCorreoSecundario" runat="server" CssClass="field-label" Text="Correo Secundario:"></asp:Label>
            <asp:TextBox ID="txtCorreoSecundario" runat="server" CssClass="field-input" onkeyup="valCorreoSec(this)"></asp:TextBox>
            <span id="spnCorreoSec" class="correo-sec-hint"></span>
        </div>

        <div class="field-inline-group">
            <div class="field">
                <asp:Label ID="lblContrasena" runat="server" CssClass="field-label" Text="Contraseña:"></asp:Label>
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="field-input"></asp:TextBox>
                <asp:Label ID="lblInfoContrasena" runat="server" CssClass="field-hint"
                    Text="(dejar vacío para mantener la actual)"></asp:Label>
            </div>
            <div class="field">
                <asp:Label ID="lblConfirmar" runat="server" CssClass="field-label" Text="Confirmar Contraseña:"></asp:Label>
                <asp:TextBox ID="txtConfirmar" runat="server" TextMode="Password" CssClass="field-input"></asp:TextBox>
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

        <div class="field-inline-group">
            <div class="field">
                <asp:Label ID="lblFechaNac" runat="server" CssClass="field-label" Text="Fecha de Nacimiento:"></asp:Label>
                <asp:TextBox ID="txtFechaNac" runat="server" TextMode="Date" CssClass="field-input"></asp:TextBox>
                <asp:CompareValidator ID="cvFechaNac" runat="server"
                    ControlToValidate="txtFechaNac" Operator="DataTypeCheck" Type="Date"
                    ErrorMessage="Ingrese una fecha válida" CssClass="field-error"
                    EnableClientScript="false"></asp:CompareValidator>
            </div>
            <div class="field">
                <asp:Label ID="lblRol" runat="server" CssClass="field-label" Text="Rol:"></asp:Label>
                <asp:DropDownList ID="ddlRol" runat="server" CssClass="field-select">
                    <asp:ListItem Text="Usuario" Value="Usuario"></asp:ListItem>
                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-panel-actions">
            <asp:Button ID="BtnGuardar" runat="server" Text="Guardar"
                OnClick="BtnGuardar_Click" CssClass="btn btn-primary" />
            <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar"
                OnClick="BtnCancelar_Click" CssClass="btn btn-secondary" />
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
    </asp:Panel>

        </div>
    </div>
</asp:Content>
