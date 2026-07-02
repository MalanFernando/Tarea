# Master Page Implementation Plan

## Strategy

Two master pages — zero changes to `C_negocio/` or `C_datos/` or logic.

| Master | Pages | Layout |
|--------|-------|--------|
| `Public.Master` | Login, Register, Recuperar, Error413 | No sidebar, minimal shell |
| `Site.Master` | Inicio, MiPerfil, AdminUsuarios, AdminProveedores, AdminProductos | Sidebar + auth guard + logout |

---

## Step 1 — toast.js (add shared `validarImagen`)

**File:** `C_presentacion/Scripts/toast.js`

Append the `validarImagen()` function after the existing `mostrarToast()`:

```js
function validarImagen(input) {
    var extensiones = ['.jpg', '.jpeg', '.png', '.gif'];
    var maxBytes = 31457280;
    if (input.files && input.files[0]) {
        var archivo = input.files[0];
        var nombre = archivo.name.toLowerCase();
        var valida = false;
        for (var i = 0; i < extensiones.length; i++) {
            if (nombre.endsWith(extensiones[i])) { valida = true; break; }
        }
        if (!valida) {
            mostrarToast('Formato no válido. Solo se permiten imágenes JPG, PNG o GIF.', 'error');
            input.value = ''; return;
        }
        if (archivo.size > maxBytes) {
            mostrarToast('La imagen es demasiado grande. El tamaño máximo es 30 MB.', 'warning');
            input.value = ''; return;
        }
    }
}
```

---

## Step 2 — Create Public.Master

**File:** `C_presentacion/Public.Master`

```aspx
<%@ Master Language="C#" AutoEventWireup="true" CodeBehind="Public.Master.cs"
    Inherits="C_presentacion.Public" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <asp:ContentPlaceHolder ID="TitleContent" runat="server">
        <title>CRUD Tienda</title>
    </asp:ContentPlaceHolder>
    <asp:ContentPlaceHolder ID="HeadContent" runat="server"></asp:ContentPlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ContentPlaceHolder ID="MainContent" runat="server"></asp:ContentPlaceHolder>
    </form>
</body>
</html>
```

**File:** `C_presentacion/Public.Master.cs`

```csharp
using System;
namespace C_presentacion
{
    public partial class Public : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e) { }
    }
}
```

**File:** `C_presentacion/Public.Master.designer.cs`

```csharp
namespace C_presentacion
{
    public partial class Public
    {
        protected global::System.Web.UI.WebControls.ContentPlaceHolder TitleContent;
        protected global::System.Web.UI.WebControls.ContentPlaceHolder HeadContent;
        protected global::System.Web.UI.WebControls.ContentPlaceHolder MainContent;
    }
}
```

---

## Step 3 — Create Site.Master

**File:** `C_presentacion/Site.Master`

```aspx
<%@ Master Language="C#" AutoEventWireup="true" CodeBehind="Site.Master.cs"
    Inherits="C_presentacion.Site" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <asp:ContentPlaceHolder ID="TitleContent" runat="server">
        <title>CRUD Tienda</title>
    </asp:ContentPlaceHolder>
    <script src="Scripts/toast.js"></script>
    <asp:ContentPlaceHolder ID="HeadContent" runat="server"></asp:ContentPlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <div class="app-layout">
            <aside class="sidebar">
                <div class="sidebar-brand">
                    <span class="brand-mark">CT</span>
                    <div class="brand-line"></div>
                </div>
                <div class="sidebar-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblAvatar" runat="server" />
                    </div>
                    <asp:Label ID="lblBienvenida" runat="server" CssClass="user-name" />
                    <asp:Label ID="lblRol" runat="server" CssClass="user-role" />
                </div>
                <nav class="sidebar-nav">
                    <asp:HyperLink ID="hlInicio" runat="server" NavigateUrl="~/Inicio.aspx" CssClass="nav-link">Inicio</asp:HyperLink>
                    <asp:HyperLink ID="hlMiPerfil" runat="server" NavigateUrl="~/MiPerfil.aspx" CssClass="nav-link">Mi Perfil</asp:HyperLink>
                    <asp:HyperLink ID="hlAdmin" runat="server" NavigateUrl="~/AdminUsuarios.aspx" CssClass="nav-link">Usuarios</asp:HyperLink>
                    <asp:HyperLink ID="hlProveedores" runat="server" NavigateUrl="~/AdminProveedores.aspx" CssClass="nav-link">Proveedores</asp:HyperLink>
                    <asp:HyperLink ID="hlProductos" runat="server" NavigateUrl="~/AdminProductos.aspx" CssClass="nav-link">Productos</asp:HyperLink>
                </nav>
                <div class="sidebar-footer">
                    <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar sesión"
                        OnClick="btnCerrarSesion_Click" CssClass="btn-logout" />
                </div>
            </aside>
            <main class="content">
                <asp:ContentPlaceHolder ID="MainContent" runat="server"></asp:ContentPlaceHolder>
            </main>
        </div>
    </form>
</body>
</html>
```

**File:** `C_presentacion/Site.Master.cs`

```csharp
using System;
namespace C_presentacion
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usu_id"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                string nombre = Session["usu_nombre"].ToString();
                lblBienvenida.Text = nombre;
                lblRol.Text = Session["usu_rol"].ToString();
                lblAvatar.Text = nombre.Length > 0 ? nombre[0].ToString().ToUpper() : "U";

                string rol = Session["usu_rol"].ToString();
                hlAdmin.Visible = (rol == "Administrador");
                hlProveedores.Visible = (rol == "Administrador" || rol == "Empleado");
                hlProductos.Visible = (rol == "Administrador" || rol == "Empleado");
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
```

**File:** `C_presentacion/Site.Master.designer.cs`

```csharp
namespace C_presentacion
{
    public partial class Site
    {
        protected global::System.Web.UI.WebControls.ContentPlaceHolder TitleContent;
        protected global::System.Web.UI.WebControls.ContentPlaceHolder HeadContent;
        protected global::System.Web.UI.WebControls.Label lblAvatar;
        protected global::System.Web.UI.WebControls.Label lblBienvenida;
        protected global::System.Web.UI.WebControls.Label lblRol;
        protected global::System.Web.UI.WebControls.HyperLink hlInicio;
        protected global::System.Web.UI.WebControls.HyperLink hlMiPerfil;
        protected global::System.Web.UI.WebControls.HyperLink hlAdmin;
        protected global::System.Web.UI.WebControls.HyperLink hlProveedores;
        protected global::System.Web.UI.WebControls.HyperLink hlProductos;
        protected global::System.Web.UI.WebControls.ContentPlaceHolder MainContent;
        protected global::System.Web.UI.WebControls.Button btnCerrarSesion;
    }
}
```

---

## Step 4 — Refactor Public Pages (Login, Register, Recuperar, Error413)

For each page, the transformation is the same:

Replace:
```aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="X.aspx.cs"
    Inherits="C_presentacion.X" ResponseEncoding="utf-8" %>
<!DOCTYPE html><html>...<head><meta ...><title>...</title></head>
<body><form>...content...</form></body></html>
```

With:
```aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="X.aspx.cs"
    Inherits="C_presentacion.X" MasterPageFile="~/Public.Master"
    ResponseEncoding="utf-8" Title="Title - CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    ...content (original inner markup, no html/body/form tags)...
</asp:Content>
```

The `Title` attribute in the `<%@ Page %>` directive sets the master's `TitleContent` placeholder.

### Login.aspx

Keep only the `<div>` content (email, password, button, links).

No designer changes needed (controls remain on the page).

### Register.aspx

Keep only form fields + button + link.

No designer changes needed.

### Recuperar.aspx

Keep only email field + button + labels + link.

No designer changes needed.

### Error413.aspx

Keep only heading + paragraphs + link.

No designer changes needed.

---

## Step 5 — Refactor Inicio.aspx

### ASPX changes:

```aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs"
    Inherits="C_presentacion.Inicio" MasterPageFile="~/Site.Master"
    ResponseEncoding="utf-8" Title="Inicio — CRUD Tienda" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.7/dist/chart.umd.min.js"></script>
    <script src="Scripts/charts.js"></script>
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <header class="page-header">
        <h1 class="page-title">Inicio</h1>
        <p class="page-subtitle">Resumen del estado de tu tienda</p>
    </header>
    <div class="stats-grid">
        <!-- 4 stat cards with <%= %> expressions -->
    </div>
    <asp:Panel ID="pnlCharts" runat="server" Visible="false" CssClass="charts-section">
        <!-- 3 chart canvases -->
    </asp:Panel>
    <script>
        var categorias = <%= GetProductosPorCategoriaJSON() %>;
        var roles = <%= GetUsuariosPorRolJSON() %>;
        var proveedores = <%= GetProductosPorProveedorJSON() %>;
        inicializarCharts(categorias, roles, proveedores);
    </script>
</asp:Content>
```

### Code-behind changes (`Inicio.aspx.cs`):

Remove:
- Entire auth check block (`if (Session["usu_id"] == null)`)
- `lblBienvenida.Text = nombre;`
- `lblRol.Text = ...;`
- `lblAvatar.Text = ...;`
- `hlAdmin.Visible = ...;`
- `hlProveedores.Visible = ...;`
- `hlProductos.Visible = ...;`
- `btnCerrarSesion_Click` method entirely

Keep:
- `pnlCharts.Visible = true;` (role-based, stays in page)
- All `GetTotal*()` and `Get*JSON()` methods

New `Page_Load` body:
```csharp
if (!IsPostBack)
{
    pnlCharts.Visible = (Session["usu_rol"].ToString() == "Administrador");
}
```

### Designer changes (`Inicio.aspx.designer.cs`):

Remove these lines (moved to Site.Master):
- `lblAvatar;`
- `lblBienvenida;`
- `lblRol;`
- `hlMiPerfil;`
- `hlAdmin;`
- `hlProveedores;`
- `hlProductos;`
- `btnCerrarSesion;`

Keep only:
- `pnlCharts;`

---

## Step 6 — Refactor MiPerfil.aspx

### ASPX changes:

```aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs"
    Inherits="C_presentacion.MiPerfil" MasterPageFile="~/Site.Master"
    ResponseEncoding="utf-8" Title="Mi Perfil — CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Mi Perfil</h2>
    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    <div class="form-content">
        <!-- Profile image upload, form fields, buttons (keep existing) -->
    </div>
</asp:Content>
```

Remove the inline `<script>validarImagen</script>` (now in toast.js), the sidebar, the form/html wrappers.

### Code-behind changes (`MiPerfil.aspx.cs`):

Remove:
- Auth check block
- `btnCerrarSesion_Click` method
- The `fuImagen.Attributes["accept"]` and `fuImagen.Attributes["onchange"]` lines (the `onchange` setup stays — it calls the shared `validarImagen()`)

Keep everything else.

### Designer changes (`MiPerfil.aspx.designer.cs`):

Remove:
- `hlInicio;`
- `hlMiPerfil;`
- `hlAdmin;`
- `hlProveedores;`
- `hlProductos;`
- `btnCerrarSesion;`

Keep all form controls.

---

## Step 7 — Refactor AdminUsuarios.aspx

### ASPX changes:

```aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs"
    Inherits="C_presentacion.AdminUsuarios" MasterPageFile="~/Site.Master"
    ResponseEncoding="utf-8" Title="Usuarios — CRUD Tienda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Usuarios</h2>
    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    <!-- GridView, buttons, form panel (keep exactly as-is) -->
</asp:Content>
```

Remove sidebar, inline `<script>validarImagen`, form/html wrappers.

### Code-behind changes (`AdminUsuarios.aspx.cs`):

Remove:
- Auth check block (`if (Session["usu_id"] == null || Session["usu_rol"].ToString() != "Administrador")`) — KEEP the role check part ONLY if you want page-level role enforcement. Actually the user said "no tocar la lógica". The role check IS logic. But it's authorization logic that prevents access. With the master handling nav visibility, a user could still navigate directly to `/AdminUsuarios.aspx`. So keep the role check.

Wait, re-reading the requirement: "Evita modificar la lógica del proyecto o la estructura raíz" and "Si debes reestructurar los archivos de acuerdo a la página maestra hazlo pero no toques la lógica".

So the role check must stay. But the session null check moves to the master. The role check is page-specific logic.

Updated: Keep the role check, remove the null check and the btnCerrarSesion_Click.

```csharp
// New auth check (keep only role validation):
if (Session["usu_rol"].ToString() != "Administrador")
{
    Response.Redirect("~/Login.aspx");
    return;
}
```

Actually wait, if the master already redirects when `Session["usu_id"] == null`, then when we get to the page code, `Session["usu_id"]` is guaranteed to exist. But `Session["usu_rol"]` could be null if the session was somehow corrupted. But we can trust the master's check. So just the role check:

```csharp
if (Session["usu_rol"].ToString() != "Administrador")
    Response.Redirect("~/Login.aspx");
```

Remove btnCerrarSesion_Click entirely.

### Designer changes (`AdminUsuarios.aspx.designer.cs`):

Remove:
- `hlInicio;`
- `hlMiPerfil;`
- `hlAdmin;`
- `hlProveedores;`
- `hlProductos;`
- `btnCerrarSesion;`

Keep all form controls.

---

## Step 8 — Refactor AdminProveedores.aspx

Same pattern as AdminUsuarios.

### ASPX:

Remove sidebar, script, html/form wrappers. Wrap content in `<asp:Content>`.

### Code-behind:

Remove the session null check (master handles it), keep the dual-role check:
```csharp
string rol = Session["usu_rol"].ToString();
if (rol != "Administrador" && rol != "Empleado")
    Response.Redirect("~/Login.aspx");
```

Remove `btnCerrarSesion_Click`.

### Designer:

Remove `hlInicio, hlMiPerfil, hlAdmin, hlProveedores, hlProductos, btnCerrarSesion`.

---

## Step 9 — Refactor AdminProductos.aspx

Identical pattern to AdminProveedores.

---

## Step 10 — Update csproj

Add these `<Content Include="..." />` entries to `C_presentacion/C_presentacion.csproj` in the appropriate section:

```xml
<Content Include="Public.Master" />
<Content Include="Site.Master" />
```

(Both use build action `MasterPageFile` implicitly or explicitly as `Content`.)

---

## Summary of all file changes

| Action | File |
|--------|------|
| Edit | `Scripts/toast.js` — append `validarImagen()` |
| Create | `Public.Master` |
| Create | `Public.Master.cs` |
| Create | `Public.Master.designer.cs` |
| Create | `Site.Master` |
| Create | `Site.Master.cs` |
| Create | `Site.Master.designer.cs` |
| Edit | `Login.aspx` — add MasterPageFile, wrap in Content |
| Edit | `Login.aspx.designer.cs` — no change |
| Edit | `Register.aspx` — same |
| Edit | `Register.aspx.designer.cs` — no change |
| Edit | `Recuperar.aspx` — same |
| Edit | `Recuperar.aspx.designer.cs` — no change |
| Edit | `Error413.aspx` — same |
| Edit | `Error413.aspx.designer.cs` — no change |
| Edit | `Inicio.aspx` — rebuild as Content |
| Edit | `Inicio.aspx.cs` — remove auth/sidebar/logout logic |
| Edit | `Inicio.aspx.designer.cs` — remove sidebar controls |
| Edit | `MiPerfil.aspx` — rebuild as Content |
| Edit | `MiPerfil.aspx.cs` — remove auth/logout |
| Edit | `MiPerfil.aspx.designer.cs` — remove sidebar controls |
| Edit | `AdminUsuarios.aspx` — rebuild as Content |
| Edit | `AdminUsuarios.aspx.cs` — remove null-check, keep role-check, remove logout |
| Edit | `AdminUsuarios.aspx.designer.cs` — remove sidebar controls |
| Edit | `AdminProveedores.aspx` — same pattern |
| Edit | `AdminProveedores.aspx.cs` — same |
| Edit | `AdminProveedores.aspx.designer.cs` — same |
| Edit | `AdminProductos.aspx` — same |
| Edit | `AdminProductos.aspx.cs` — same |
| Edit | `AdminProductos.aspx.designer.cs` — same |
| Edit | `C_presentacion.csproj` — add master page includes |

---

## Rollback plan

To revert: restore all files from git (`git checkout -- .` in the presentation directory). Master pages and toast.js changes can be reverted individually.
