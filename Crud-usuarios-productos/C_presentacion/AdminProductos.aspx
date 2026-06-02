<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminProductos.aspx.cs" Inherits="C_presentacion.AdminProductos" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de Productos - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/admin-productos.css" />
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
            <h2>Administración de Productos</h2>
            <asp:HyperLink ID="hlInicio" runat="server" NavigateUrl="~/Inicio.aspx">Volver al Inicio</asp:HyperLink>
            <br /><br />

            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <br />

            <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="false"
                OnRowCommand="gvProductos_RowCommand" CellPadding="5" DataKeyNames="pr_id">
                <Columns>
                    <asp:BoundField DataField="pr_id" HeaderText="ID" />
                    <asp:BoundField DataField="pr_nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="pr_precio" HeaderText="Precio" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                    <asp:TemplateField HeaderText="Imagen">
                        <ItemTemplate>
                            <asp:Image ID="imgProducto" runat="server"
                                ImageUrl='<%# Eval("imagen_portada") %>'
                                Visible='<%# Eval("imagen_portada") != DBNull.Value && !string.IsNullOrEmpty(Eval("imagen_portada").ToString()) %>'
                                CssClass="producto-thumb" />
                            <span class="sin-imagen"
                                Visible='<%# Eval("imagen_portada") == DBNull.Value || string.IsNullOrEmpty(Eval("imagen_portada").ToString()) %>'
                                runat="server">(sin imagen)</span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField Text="Editar" CommandName="Editar" ButtonType="Button" />
                    <asp:ButtonField Text="Eliminar" CommandName="Eliminar" ButtonType="Button" />
                </Columns>
            </asp:GridView>

            <br />
            <asp:Button ID="btnNuevo" runat="server" Text="Agregar Nuevo Producto" OnClick="btnNuevo_Click" />
            <br /><br />

            <asp:Panel ID="pnlFormulario" runat="server" Visible="false">
                <h3><asp:Label ID="lblTituloFormulario" runat="server" Text="Agregar Producto"></asp:Label></h3>

                <asp:HiddenField ID="hfProductoId" runat="server" Value="0" />

                <asp:Label ID="lblNombre" runat="server" Text="Nombre del Producto:"></asp:Label>
                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                    ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                    ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
                <br /><br />

                <asp:Label ID="lblDescripcion" runat="server" Text="Descripción:"></asp:Label>
                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" Columns="40"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
                    ControlToValidate="txtDescripcion" ErrorMessage="La descripción es obligatoria"
                    ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
                <br /><br />

                <asp:Label ID="lblPrecio" runat="server" Text="Precio:"></asp:Label>
                <asp:TextBox ID="txtPrecio" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPrecio" runat="server"
                    ControlToValidate="txtPrecio" ErrorMessage="El precio es obligatorio"
                    ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvPrecio" runat="server"
                    ControlToValidate="txtPrecio" Operator="DataTypeCheck" Type="Double"
                    ErrorMessage="El precio debe ser un número válido"
                    ForeColor="Red" EnableClientScript="false"></asp:CompareValidator>
                <br /><br />

                <asp:Label ID="lblCategoria" runat="server" Text="Categoría:"></asp:Label>
                <asp:DropDownList ID="ddlCategoria" runat="server"></asp:DropDownList>
                <br /><br />

                <asp:Label ID="lblProveedor" runat="server" Text="Proveedor:"></asp:Label>
                <asp:DropDownList ID="ddlProveedor" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvProveedor" runat="server"
                    ControlToValidate="ddlProveedor" InitialValue=""
                    ErrorMessage="Seleccione un proveedor"
                    ForeColor="Red" EnableClientScript="false"></asp:RequiredFieldValidator>
                <br /><br />

                <asp:Label ID="lblImagenes" runat="server" Text="Imágenes (máximo 3):"></asp:Label>
                <br />
                <asp:GridView ID="gvImagenes" runat="server" AutoGenerateColumns="false"
                    OnRowCommand="gvImagenes_RowCommand" DataKeyNames="img_id"
                    ShowHeader="false" GridLines="None" CellPadding="5">
                    <Columns>
                        <asp:ImageField DataImageUrlField="img_ruta" ControlStyle-Width="80" ControlStyle-Height="80" />
                        <asp:ButtonField Text="Quitar" CommandName="Quitar" ButtonType="Button" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblSinImagenes" runat="server" Text="(sin imágenes)" Font-Italic="true"></asp:Label>
                <br /><br />

                <asp:FileUpload ID="fuImagen" runat="server" />
                <asp:Button ID="btnCargarImagen" runat="server" Text="Agregar Imagen" OnClick="btnCargarImagen_Click" />
                <br />
                <asp:Label ID="lblInfoImagen" runat="server"
                    Text="(formatos: JPG, PNG, GIF | máximo 30 MB)"
                    ForeColor="Gray" Font-Italic="true"></asp:Label>
                <br /><br />

                <asp:HiddenField ID="hfNuevasImagenes" runat="server" Value="" />
                <asp:HiddenField ID="hfImagenesAEliminar" runat="server" Value="" />

                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
