<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs" Inherits="C_presentacion.AdminUsuarios" ResponseEncoding="utf-8" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Administración de Usuarios - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/admin-usuarios.css" />
    <link rel="stylesheet" type="text/css" href="Styles/toast.css" />
    <script src="Scripts/toast.js"></script>
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
                    mostrarToast('Formato no válido. Solo se permiten imágenes JPG, PNG o GIF.', 'error');
                    input.value = '';
                    return;
                }

                if (archivo.size > maxBytes) {
                    mostrarToast('La imagen es demasiado grande. El tamaño máximo es 30 MB.', 'warning');
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
            <nav class="sidebar">
            <asp:HyperLink ID="hlMiPerfil" runat="server" NavigateUrl="~/MiPerfil.aspx">Mi Perfil</asp:HyperLink>

            <asp:HyperLink ID="hlAdmin" runat="server" NavigateUrl="~/AdminUsuarios.aspx" Visible="true">Panel de Administración de Usuarios</asp:HyperLink>

            <asp:HyperLink ID="hlProveedores" runat="server" NavigateUrl="~/AdminProveedores.aspx" Visible="true">Administrar Proveedores</asp:HyperLink>

            <asp:HyperLink ID="hlProductos" runat="server" NavigateUrl="~/AdminProductos.aspx" Visible="true">Administrar Productos</asp:HyperLink>

            <asp:HyperLink ID="hlInicio" runat="server" NavigateUrl="~/Inicio.aspx">Volver al Inicio</asp:HyperLink>

            <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" OnClick="btnCerrarSesion_Click" />
            </nav>
            <main class="content">
            <h2>Administración de Usuarios</h2>

            <!-- Mensaje de feedback -->
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>


            <!-- GridView con la lista de usuarios -->
            <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false"
                OnRowCommand="gvUsuarios_RowCommand" CellPadding="5" DataKeyNames="usu_id">
                <Columns>
                    <asp:BoundField DataField="usu_id" HeaderText="ID" />
                    <asp:BoundField DataField="usu_nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="usu_correo" HeaderText="Correo" />
                    <asp:BoundField DataField="usu_cedula" HeaderText="Cédula" />
                    <asp:BoundField DataField="usu_celular" HeaderText="Celular" />
                    <asp:BoundField DataField="usu_rol" HeaderText="Rol" />
                    <asp:BoundField DataField="usu_estado" HeaderText="Estado" />
                    <asp:BoundField DataField="usu_intentos" HeaderText="Intentos" />
                    <asp:ButtonField Text="Editar" CommandName="Editar" ButtonType="Button" />
                    <asp:ButtonField Text="Activar/Desactivar" CommandName="CambiarEstado" ButtonType="Button" />
                </Columns>
            </asp:GridView>

            
            <asp:Button ID="btnNuevo" runat="server" Text="Agregar Nuevo Usuario" OnClick="btnNuevo_Click" />
            

            <!-- Panel para agregar o editar usuario -->
            <asp:Panel ID="pnlFormulario" runat="server" Visible="false">
                <h3><asp:Label ID="lblTituloFormulario" runat="server" Text="Agregar Usuario"></asp:Label></h3>

                <!-- Campo oculto para guardar el ID del usuario en edición -->
                <asp:HiddenField ID="hfUsuarioId" runat="server" Value="0" />

                <!-- Nombre -->
                <asp:Label ID="lblNombre" runat="server" Text="Nombre Completo:"></asp:Label>
                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                    ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                    ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
                

                <!-- Correo -->
                <asp:Label ID="lblCorreo" runat="server" Text="Correo Electrónico:"></asp:Label>
                <asp:TextBox ID="txtCorreo" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                    ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                    ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
                

                <!-- Contraseña (solo obligatoria para nuevo usuario) -->
                <asp:Label ID="lblContrasena" runat="server" Text="Contraseña:"></asp:Label>
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password"></asp:TextBox>
                <asp:Label ID="lblInfoContrasena" runat="server" Text="(dejar vacío para mantener la actual)"
                    ForeColor="Gray" Font-Italic="true"></asp:Label>
                

                <!-- Confirmar contraseña -->
                <asp:Label ID="lblConfirmar" runat="server" Text="Confirmar Contraseña:"></asp:Label>
                <asp:TextBox ID="txtConfirmar" runat="server" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="cvContrasenas" runat="server"
                    ControlToValidate="txtConfirmar" ControlToCompare="txtContrasena"
                    ErrorMessage="Las contraseñas no coinciden" ForeColor="Red"
                    EnableClientScript="false"></asp:CompareValidator>
                

                <!-- Cédula -->
                <asp:Label ID="lblCedula" runat="server" Text="Cédula (opcional):"></asp:Label>
                <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
                

                <!-- Celular -->
                <asp:Label ID="lblCelular" runat="server" Text="Celular (opcional):"></asp:Label>
                <asp:TextBox ID="txtCelular" runat="server"></asp:TextBox>
                

                <!-- Rol -->
                <asp:Label ID="lblRol" runat="server" Text="Rol:"></asp:Label>
                <asp:DropDownList ID="ddlRol" runat="server">
                    <asp:ListItem Text="Empleado" Value="Empleado"></asp:ListItem>
                    <asp:ListItem Text="Administrador" Value="Administrador"></asp:ListItem>
                </asp:DropDownList>
                

                <!-- Imagen de perfil -->
                <asp:Label ID="lblImagen" runat="server" Text="Foto de perfil:"></asp:Label>
                
                <asp:Image ID="imgPerfil" runat="server" Width="100" Height="100" Visible="false" />
                <asp:Label ID="lblSinImagen" runat="server" Text="(sin imagen)" Font-Italic="true"
                    Visible="false"></asp:Label>
                

                <asp:FileUpload ID="fuImagen" runat="server" />
                <asp:Button ID="btnCargarImagen" runat="server" Text="Cargar Imagen" OnClick="btnCargarImagen_Click" />
                
                <asp:Label ID="lblInfoImagen" runat="server"
                    Text="(formatos: JPG, PNG, GIF | máximo 30 MB)"
                    ForeColor="Gray" Font-Italic="true"></asp:Label>
                

                <asp:HiddenField ID="hfNuevaImagen" runat="server" Value="" />

                <!-- Botones Guardar / Cancelar -->
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
            </asp:Panel>
            </main>
        </div>
    </form>
</body>
</html>