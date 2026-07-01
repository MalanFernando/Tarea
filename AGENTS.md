# AGENTS.md — CRUD Productos (.NET Framework 4.8.1)

## Quick start

```powershell
# Run from Crud-usuarios-productos/ directory:
nuget restore C_presentacion.sln
msbuild C_presentacion.sln /p:Configuration=Debug
```

Run via Visual Studio (IIS Express) — port **22447** — or deploy to local IIS.

## Architecture

- `C_presentacion/` — ASP.NET Web Forms app, .NET 4.8.1, entrypoint is `Login.aspx` (default document)
- `C_negocio/` — Business logic, references `C_datos`
- `C_datos/` — Data access, contains the SQL connection string (hardcoded)
- Dependency: `presentacion → negocio → datos`

## Key details

| What | Where |
|------|-------|
| DB script | `tienda.sql` (repo root) — creates `CRUD_Tienda` DB on SQL Server |
| Connection string | `C_datos/Conexion.cs:9` — `DESKTOP-E1BDAD6\SQLEXPRESS`, Windows Auth |
| Password encryption | `ENCRYPTBYPASSPHRASE` / `DECRYPTBYPASSPHRASE` with key `cl@ve` |
| Seed users | `admin@admin.com` / `admin123` (Admin), `empleado@ejemplo.com` / `empleado123` (Empleado) |
| Roles | `Administrador`, `Empleado` — CHECK constraint on `usu_rol` |

- **Windows-only** — .NET Framework 4.8.1, IIS Express, SQL Server. Not compatible with `dotnet` CLI; use `msbuild`.
- **No tests, no linters, no CI/CD.**
- **Upload limit**: 35 MB (`Web.config`: `httpRuntime maxRequestLength=35840` + `requestLimits maxAllowedContentLength=36700160`). Exceeding it redirects to `Error413.aspx`.
- **Uploaded files** stored in `Imagenes/Perfiles/` (profile photos) and `Imagenes/Productos/` (product images).

## Pages

| Page | Access | Purpose |
|------|--------|---------|
| `Login.aspx` | Public | Email + password auth; blocks after 3 failed attempts (sets `usu_estado='B'`) |
| `Register.aspx` | Public | Self-registration with validations |
| `Recuperar.aspx` | Public | Email → shows decrypted password in plain text |
| `Inicio.aspx` | Authenticated | Landing page; admin sees link to admin panel |
| `AdminUsuarios.aspx` | Admin only | CRUD users, enable/disable, reset passwords |
| `AdminProveedores.aspx` | Admin + Empleado | CRUD proveedores with optional image |
| `AdminProductos.aspx` | Admin + Empleado | CRUD productos with up to 3 images |
| `MiPerfil.aspx` | Authenticated | Edit name/cedula/celular, upload profile photo |
| `Error413.aspx` | — | Displayed when upload exceeds 35 MB limit |

- Session keys: `usu_id`, `usu_nombre`, `usu_correo`, `usu_rol`
- Role checks happen in `Page_Load`; unauthorized users are redirected
- All validators use `EnableClientScript="false"` — server-side only
