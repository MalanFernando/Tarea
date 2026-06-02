# AGENTS.md — CRUD Productos (.NET Framework 4.8.1)

## Quick start

```powershell
# Restore NuGet & build (from Crud-usuarios-productos/)
nuget restore C_presentacion.sln
msbuild C_presentacion.sln /p:Configuration=Debug
```

Run from Visual Studio (IIS Express) or deploy to local IIS.

## Architecture

- `C_presentacion/` — ASP.NET Web Forms app, targets .NET Framework 4.8.1, IIS Express port **22447**
- `C_negocio/` — Business logic class library, references `C_datos`
- `C_datos/` — Data access class library, contains `Conexion.cs` with the SQL connection string
- Dependency chain: `presentacion → negocio → datos`

## Database

- SQL Server script: `tienda.sql` (repo root) — creates `CRUD_Tienda` DB
- Connection string hardcoded in `C_datos/Conexion.cs:12` — points to `BUMBLEBEE\SQLEXPRESS` (Windows Auth)
- Key tables: `tbl_usuario`, `tbl_proveedor`, `tbl_categoria`, `tbl_producto`, `tbl_producto_imagen`
- Passwords encrypted via `ENCRYPTBYPASSPHRASE` (key: `cl@ve`)
- Seed users: `admin@admin.com` / `admin123` (Administrador), `empleado@ejemplo.com` / `empleado123` (Empleado)
- Roles: `Administrador`, `Empleado` (CHECK constraint on `usu_rol`)

## Important notes

- **Windows-only** — .NET Framework 4.8.1, IIS Express, SQL Server. Not compatible with `dotnet` CLI (uses `msbuild`).
- Only NuGet package: `Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1`
- No tests, no linters, no CI/CD configured.

## Pages / flow

| Page | Purpose |
|------|---------|
| `Login.aspx` | Authenticate with email + password. Tracks failed attempts (blocks after 3). |
| `Register.aspx` | New user registration with validations. |
| `Recuperar.aspx` | Enter email → desencripta y muestra la contraseña en texto plano. |
| `Inicio.aspx` | Landing page after login; shows user name/role. Shows link to admin panel if `Administrador`. Logout clears session. |
| `AdminUsuarios.aspx` | Admin-only: CRUD users, enable/disable accounts, reset passwords. |
| `AdminProveedores.aspx` | Admin y Empleado: CRUD proveedores con imagen opcional. |
| `AdminProductos.aspx` | Admin y Empleado: CRUD productos con hasta 3 imágenes. |
| `MiPerfil.aspx` | All users: edit name/cedula/celular, upload profile photo with server-side preview. |

- Session stores `usu_id`, `usu_nombre`, `usu_correo`, `usu_rol` after login.
- `AdminUsuarios.aspx` checks `Session["usu_rol"] == "Administrador"` on load — non-admins are redirected.
- `AdminProveedores.aspx` and `AdminProductos.aspx` check `Session["usu_rol"]` for `Administrador` or `Empleado`.
- All validators use `EnableClientScript="false"` — validation runs server-side only.
