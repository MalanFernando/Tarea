<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="C_presentacion.Inicio" MasterPageFile="~/MasterPages/Site.Master" ResponseEncoding="utf-8" Title="Inicio — CRUD Tienda" %>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-welcome">
        <h1>Bienvenido, <%= Session["usu_nombre"] %></h1>
        <p>Rol: <%= Session["usu_rol"] %></p>
        <p>Ha iniciado sesión correctamente en el sistema.</p>

        <div class="card" style="margin-top:1.5rem">
            <p style="margin-bottom:0">Seleccione una opción en el menú lateral para comenzar.</p>
        </div>
    </div>
</asp:Content>
