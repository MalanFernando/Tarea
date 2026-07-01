<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="C_presentacion.Inicio" ResponseEncoding="utf-8" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Inicio - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/inicio.css" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.7/dist/chart.umd.min.js"></script>
    <script src="Scripts/charts.js"></script>
    <style>
        .chart-container {
            background: #fff;
            border: 1px solid #e3e6f0;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 20px;
            box-shadow: 0 0.15rem 1.75rem rgba(58,59,69,0.1);
        }
        .chart-container h4 {
            margin: 0 0 15px 0;
            color: #5a5c69;
            font-size: 1rem;
            border-bottom: 1px solid #e3e6f0;
            padding-bottom: 10px;
        }
        .chart-row {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
        }
        .chart-row .chart-container {
            flex: 1;
            min-width: 280px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <nav class="sidebar">
            <asp:Label ID="lblBienvenida" runat="server"></asp:Label>

            <asp:Label ID="lblRol" runat="server"></asp:Label>


            <asp:HyperLink ID="hlMiPerfil" runat="server" NavigateUrl="~/MiPerfil.aspx">Mi Perfil</asp:HyperLink>


            <asp:HyperLink ID="hlAdmin" runat="server" NavigateUrl="~/AdminUsuarios.aspx" Visible="false">Panel de Administración de Usuarios</asp:HyperLink>


            <asp:HyperLink ID="hlProveedores" runat="server" NavigateUrl="~/AdminProveedores.aspx" Visible="false">Administrar Proveedores</asp:HyperLink>


            <asp:HyperLink ID="hlProductos" runat="server" NavigateUrl="~/AdminProductos.aspx" Visible="false">Administrar Productos</asp:HyperLink>

            <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" OnClick="btnCerrarSesion_Click" />
            </nav>
            <main class="content">
            <h2>Bienvenido al Sistema CRUD Tienda</h2>

            <asp:Panel ID="pnlCharts" runat="server" Visible="false">
                <h3>Panel de control</h3>

                <div class="chart-row">
                    <div class="chart-container">
                        <h4>Productos por Categoría</h4>
                        <canvas id="chartCategorias" height="200"></canvas>
                    </div>
                    <div class="chart-container">
                        <h4>Usuarios por Rol</h4>
                        <canvas id="chartRoles" height="200"></canvas>
                    </div>
                </div>

                <div class="chart-row">
                    <div class="chart-container">
                        <h4>Productos por Proveedor</h4>
                        <canvas id="chartProveedores" height="200"></canvas>
                    </div>
                </div>
            </asp:Panel>
            </main>
        </div>
    </form>

    <script>
        var categorias = <%= GetProductosPorCategoriaJSON() %>;
        var roles = <%= GetUsuariosPorRolJSON() %>;
        var proveedores = <%= GetProductosPorProveedorJSON() %>;
        inicializarCharts(categorias, roles, proveedores);
    </script>
</body>
</html>