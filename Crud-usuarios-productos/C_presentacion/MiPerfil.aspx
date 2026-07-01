<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="C_presentacion.MiPerfil" ResponseEncoding="utf-8" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Mi Perfil - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/mi-perfil.css" />
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
            <asp:Image ID="imgPerfil" runat="server" Width="150" Height="150" Visible="false"
                AlternateText="Foto de perfil" />
            <asp:Label ID="lblSinImagen" runat="server" Text="(Sin imagen de perfil)" Font-Italic="true"
                Visible="false"></asp:Label>


            <asp:HyperLink ID="hlInicio" runat="server" NavigateUrl="~/Inicio.aspx">Volver al Inicio</asp:HyperLink>
            
            <asp:HyperLink ID="hlMiPerfil" runat="server" NavigateUrl="~/MiPerfil.aspx">Mi Perfil</asp:HyperLink>
            
            <asp:HyperLink ID="hlAdmin" runat="server" NavigateUrl="~/AdminUsuarios.aspx" Visible="true">Panel de Administración de Usuarios</asp:HyperLink>
            
            <asp:HyperLink ID="hlProveedores" runat="server" NavigateUrl="~/AdminProveedores.aspx" Visible="true">Administrar Proveedores</asp:HyperLink>
            
            <asp:HyperLink ID="hlProductos" runat="server" NavigateUrl="~/AdminProductos.aspx" Visible="true">Administrar Productos</asp:HyperLink>
            
            <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" OnClick="btnCerrarSesion_Click" />
            </nav>
            <main class="content">
            <h2>Mi Perfil</h2>

            <!-- Mensaje de feedback -->
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>

            <div class="form-content">

            <!-- Carga de nueva imagen -->
            <asp:Label ID="lblSubir" runat="server" Text="Cambiar foto de perfil:"></asp:Label>
            <asp:FileUpload ID="fuImagen" runat="server" />
            <asp:Button ID="btnCargar" runat="server" Text="Cargar Imagen" OnClick="btnCargar_Click" />

            <asp:Label ID="lblInfoImagen" runat="server" Text="(formatos permitidos: jpg, jpeg, png, gif)"
                ForeColor="Gray" Font-Italic="true"></asp:Label>

            <!-- Campo oculto para guardar la ruta de la nueva imagen -->
            <asp:HiddenField ID="hfNuevaImagen" runat="server" Value="" />

            <!-- Nombre -->
            <asp:Label ID="lblNombre" runat="server" Text="Nombre Completo:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>

            <!-- Correo (solo lectura) -->
            <asp:Label ID="lblCorreo" runat="server" Text="Correo Electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server" ReadOnly="true" BackColor="#EEEEEE"></asp:TextBox>

            <!-- Cédula -->
            <asp:Label ID="lblCedula" runat="server" Text="Cédula:"></asp:Label>
            <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>

            <!-- Celular -->
            <asp:Label ID="lblCelular" runat="server" Text="Celular:"></asp:Label>
            <asp:TextBox ID="txtCelular" runat="server"></asp:TextBox>

            <!-- Botones -->
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" OnClick="btnGuardar_Click" />
            </div>
            </main>
        </div>
    </form>
</body>
</html>