<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error413.aspx.cs" Inherits="C_presentacion.Error413" ResponseEncoding="utf-8" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Archivo demasiado grande - CRUD Tienda</title>
    <link rel="stylesheet" type="text/css" href="Styles/base.css" />
    <link rel="stylesheet" type="text/css" href="Styles/error413.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Archivo demasiado grande</h2>
            <p>El archivo que intenta subir supera el tamaño máximo permitido de <strong>35 MB</strong>.</p>
            <p>Seleccione una imagen de menor tamaño e intente nuevamente.</p>
            
            <asp:HyperLink ID="hlVolver" runat="server" NavigateUrl="~/MiPerfil.aspx">Volver a Mi Perfil</asp:HyperLink>
        </div>
    </form>
</body>
</html>