<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminProveedores.aspx.cs" Inherits="C_presentacion.AdminProveedores" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de Proveedores - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/admin-proveedores.css" />
    <script>
        function validarImagen(input) {
            var extensiones = ['.jpg', '.jpeg', '.png', '.gif'];
            var maxBytes = 31457280;

            if (input.files && input.files[0]) {
                var archivo = input.files[0];
                var nombre = archivo.name.toLowerCase();
                var valida = false;

                for (var i = 0; i < extensiones.length; i++) {
                    if (nombre.endsWith(extensiones[i])) {
                        valida = true;
                        break;
                    }
                }

                if (!valida) {
                    alert('Formato no válido. Solo se permiten imágenes JPG, PNG o GIF.');
                    input.value = '';
                    return;
                }

                if (archivo.size > maxBytes) {
                    alert('La imagen es demasiado grande. El tamaño máximo es 30 MB.');
                    input.value = '';
                    return;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Administración de Proveedores</h2>
            <asp:HyperLink ID="hlInicio" runat="server" NavigateUrl="~/Inicio.aspx">Volver al Inicio</asp:HyperLink>
            <br /><br />

            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <br />

            <asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="false"
                OnRowCommand="gvProveedores_RowCommand" CellPadding="5" DataKeyNames="prv_id">
                <Columns>
                    <asp:BoundField DataField="prv_id" HeaderText="ID" />
                    <asp:BoundField DataField="prv_nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="prv_contacto" HeaderText="Contacto" />
                    <asp:BoundField DataField="prv_telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="prv_correo" HeaderText="Correo" />
                    <asp:ButtonField Text="Editar" CommandName="Editar" ButtonType="Button" />
                    <asp:ButtonField Text="Eliminar" CommandName="Eliminar" ButtonType="Button" />
                </Columns>
            </asp:GridView>

            <br />
            <asp:Button ID="btnNuevo" runat="server" Text="Agregar Nuevo Proveedor" OnClick="btnNuevo_Click" />
            <br /><br />

            <asp:Panel ID="pnlFormulario" runat="server" Visible="false">
                <h3><asp:Label ID="lblTituloFormulario" runat="server" Text="Agregar Proveedor"></asp:Label></h3>

                <asp:HiddenField ID="hfProveedorId" runat="server" Value="0" />

                <asp:Label ID="lblNombre" runat="server" Text="Nombre del Proveedor:"></asp:Label>
                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                    ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                    ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
                <br /><br />

                <asp:Label ID="lblContacto" runat="server" Text="Persona de Contacto (opcional):"></asp:Label>
                <asp:TextBox ID="txtContacto" runat="server"></asp:TextBox>
                <br /><br />

                <asp:Label ID="lblTelefono" runat="server" Text="Teléfono (opcional):"></asp:Label>
                <asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
                <br /><br />

                <asp:Label ID="lblCorreo" runat="server" Text="Correo (opcional):"></asp:Label>
                <asp:TextBox ID="txtCorreo" runat="server"></asp:TextBox>
                <br /><br />

                <asp:Label ID="lblImagen" runat="server" Text="Imagen (opcional):"></asp:Label>
                <br />
                <asp:Image ID="imgProveedor" runat="server" Width="100" Height="100" Visible="false" />
                <asp:Label ID="lblSinImagen" runat="server" Text="(sin imagen)" Font-Italic="true"
                    Visible="false"></asp:Label>
                <br /><br />

                <asp:FileUpload ID="fuImagen" runat="server" />
                <asp:Button ID="btnCargarImagen" runat="server" Text="Cargar Imagen" OnClick="btnCargarImagen_Click" />
                <br />
                <asp:Label ID="lblInfoImagen" runat="server"
                    Text="(formatos: JPG, PNG, GIF | máximo 30 MB)"
                    ForeColor="Gray" Font-Italic="true"></asp:Label>
                <br /><br />

                <asp:HiddenField ID="hfNuevaImagen" runat="server" Value="" />

                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
