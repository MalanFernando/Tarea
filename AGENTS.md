# AGENTS.md — CRUD Usuarios (.NET Framework 4.8.1)

## Quick start

```powershell
nuget restore C_presentacion.sln
msbuild sisTarea3/C_presentacion.sln /p:Configuration=Debug

```powershell
nuget restore sisTarea3/C_presentacion.sln
msbuild sisTarea3/C_presentacion.sln /p:Configuration=Debug
```

Run via Visual Studio (IIS Express) — port **22447** — or deploy to local IIS.

## Architecture

- `C_presentacion/` — ASP.NET Web Forms app, .NET 4.8.1, entrypoint is `Login.aspx`
- `C_negocio/` — Business logic, references `C_datos`
- `C_datos/` — Data access, contains the SQL connection string
- Dependency: `presentacion → negocio → datos`

## Key details

| What | Where |
|------|-------|
| DB script | `BaseTarea3.sql` (repo root) — creates `BaseTarea3` DB on SQL Server |
| Connection string | `C_datos/Conexion.cs:9` — `.\SQLEXPRESS`, Windows Auth |
| Password encryption | `ENCRYPTBYPASSPHRASE` / `DECRYPTBYPASSPHRASE` with key `cl@ve` |
| Seed users | `admin@admin.com` / `admin123` (Admin), `usuario@ejemplo.com` / `usuario123` (Usuario) |
| Roles | `Admin`, `Usuario` — CHECK constraint on `usu_rol` |
| Secondary email | `usu_correo_secundario VARCHAR(100) NULL` — optional, REGEX validated (ASCII only, no ñ/tildes) |

- **Windows-only** — .NET Framework 4.8.1, IIS Express, SQL Server.

## Pages

| Page | Access | Purpose |
|------|--------|---------|
| `Login.aspx` | Public | Email + password auth; blocks after 3 failed attempts (sets `usu_estado='B'`) |
| `Register.aspx` | Public | Self-registration (always creates role `Usuario`) |
| `Recuperar.aspx` | Public | Email → shows decrypted password in plain text |
| `Inicio.aspx` | Authenticated | Landing page |
| `AdminUsuarios.aspx` | Admin only | CRUD users, enable/disable, reset passwords |
| `MiPerfil.aspx` | Authenticated | Edit profile, view birth date in 3 formats with calculated age (Usuario only) |

- Session keys: `usu_id`, `usu_nombre`, `usu_correo`, `usu_rol`
- Role checks happen in `Page_Load`; unauthorized users are redirected
- `MiPerfil.aspx` shows birth date in 3 formats (1 long, 2 short) and calculated age only for role `Usuario`
- All validators use `EnableClientScript="false"` — server-side only
