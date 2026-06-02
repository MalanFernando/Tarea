<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="C_presentacion.MiPerfil" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mi Perfil - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/mi-perfil.css" />
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
            <h2>Mi Perfil</h2>
            <asp:HyperLink ID="hlInicio" runat="server" NavigateUrl="~/Inicio.aspx">Volver al Inicio</asp:HyperLink>
            <br /><br />

            <!-- Mensaje de feedback -->
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <br />

            <!-- Imagen de perfil actual -->
            <asp:Image ID="imgPerfil" runat="server" Width="150" Height="150" Visible="false"
                AlternateText="Foto de perfil" />
            <asp:Label ID="lblSinImagen" runat="server" Text="(Sin imagen de perfil)" Font-Italic="true"
                Visible="false"></asp:Label>
            <br /><br />

            <!-- Carga de nueva imagen -->
            <asp:Label ID="lblSubir" runat="server" Text="Cambiar foto de perfil:"></asp:Label>
            <asp:FileUpload ID="fuImagen" runat="server" />
            <asp:Button ID="btnCargar" runat="server" Text="Cargar Imagen" OnClick="btnCargar_Click" />
            <br />
            <asp:Label ID="lblInfoImagen" runat="server" Text="(formatos permitidos: jpg, jpeg, png, gif)"
                ForeColor="Gray" Font-Italic="true"></asp:Label>
            <br /><br />

            <!-- Campo oculto para guardar la ruta de la nueva imagen -->
            <asp:HiddenField ID="hfNuevaImagen" runat="server" Value="" />

            <!-- Nombre -->
            <asp:Label ID="lblNombre" runat="server" Text="Nombre Completo:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
            <br /><br />

            <!-- Correo (solo lectura) -->
            <asp:Label ID="lblCorreo" runat="server" Text="Correo Electrónico:"></asp:Label>
            <asp:TextBox ID="txtCorreo" runat="server" ReadOnly="true" BackColor="#EEEEEE"></asp:TextBox>
            <br /><br />

            <!-- Cédula -->
            <asp:Label ID="lblCedula" runat="server" Text="Cédula:"></asp:Label>
            <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
            <br /><br />

            <!-- Celular -->
            <asp:Label ID="lblCelular" runat="server" Text="Celular:"></asp:Label>
            <asp:TextBox ID="txtCelular" runat="server"></asp:TextBox>
            <br /><br />

            <!-- Botones -->
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" OnClick="btnGuardar_Click" />
        </div>
    </form>
</body>
</html>