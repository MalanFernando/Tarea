using System;
using System.Data;

namespace C_negocio
{
    // Capa de negocio que contiene la lógica para la gestión de usuarios
    // Se comunica con la capa de datos (C_datos) para realizar operaciones en la BD
    public class UsuarioNegocio
    {
        private C_datos.UsuarioDatos usuarioDatos = new C_datos.UsuarioDatos();

        // Autentica al usuario con correo y contraseña
        // Retorna un DataTable con los datos del usuario si las credenciales son correctas
        public DataTable Login(string correo, string contrasena)
        {
            return usuarioDatos.ValidarUsuario(correo, contrasena);
        }

        // Registra un nuevo usuario en el sistema
        // Retorna el ID del usuario creado
        public int Registrar(string nombre, string correo, string contrasena, string cedula, string celular, string rol)
        {
            return usuarioDatos.RegistrarUsuario(nombre, correo, contrasena, cedula, celular, rol);
        }

        // Verifica si una cédula ya está registrada
        public bool CedulaExiste(string cedula)
        {
            return usuarioDatos.CedulaExiste(cedula);
        }

        // Verifica si una cédula ya está registrada excluyendo un ID (para ediciones)
        public bool CedulaExiste(string cedula, int excludeUserId)
        {
            return usuarioDatos.CedulaExiste(cedula, excludeUserId);
        }

        // Verifica si un correo electrónico ya está registrado
        public bool CorreoExiste(string correo)
        {
            return usuarioDatos.CorreoExiste(correo);
        }

        // Obtiene los datos de un usuario por su correo electrónico
        public DataTable ObtenerUsuarioPorCorreo(string correo)
        {
            return usuarioDatos.ObtenerUsuarioPorCorreo(correo);
        }

        // Genera un token único para recuperación de contraseña y lo almacena en la BD
        public string GenerarTokenRecuperacion(int usuId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime expiracion = DateTime.Now.AddHours(1); // Token válido por 1 hora
            usuarioDatos.GuardarTokenRecuperacion(usuId, token, expiracion);
            return token;
        }

        // Valida si un token de recuperación es válido y no ha expirado
        public DataTable ValidarToken(string token)
        {
            return usuarioDatos.ValidarToken(token);
        }

        // Cambia la contraseña del usuario y limpia el token de recuperación
        public void CambiarContrasena(int usuId, string nuevaContrasena)
        {
            usuarioDatos.ActualizarContrasena(usuId, nuevaContrasena);
        }

        // Desencripta y retorna la contraseña de un usuario por su correo
        public string RecuperarContrasena(string correo)
        {
            return usuarioDatos.ObtenerContrasenaPorCorreo(correo);
        }

        // Incrementa los intentos fallidos y bloquea al usuario si llega a 3 intentos
        public void ManejarIntentoFallido(int usuId, int intentosActuales)
        {
            int nuevosIntentos = intentosActuales + 1;
            if (nuevosIntentos >= 3)
            {
                usuarioDatos.BloquearUsuario(usuId);
            }
            usuarioDatos.ActualizarIntentos(usuId, nuevosIntentos);
        }

        // Reinicia el contador de intentos fallidos (cuando el usuario inicia sesión correctamente)
        public void ReiniciarIntentos(int usuId)
        {
            usuarioDatos.ActualizarIntentos(usuId, 0);
        }

        public DataTable ObtenerUsuariosPorRol()
        {
            return usuarioDatos.ObtenerUsuariosPorRol();
        }

        // Obtiene la lista completa de usuarios
        public DataTable ListarUsuarios()
        {
            return usuarioDatos.ListarUsuarios();
        }

        // Obtiene un usuario por su ID
        public DataTable ObtenerUsuarioPorId(int usuId)
        {
            return usuarioDatos.ObtenerUsuarioPorId(usuId);
        }

        // Actualiza los datos de un usuario
        public void ActualizarUsuario(int usuId, string nombre, string correo, string cedula, string celular, string rol)
        {
            usuarioDatos.ActualizarUsuario(usuId, nombre, correo, cedula, celular, rol);
        }

        // Cambia el estado de un usuario (A = Activo, I = Inactivo)
        public void CambiarEstadoUsuario(int usuId, string estado)
        {
            usuarioDatos.CambiarEstadoUsuario(usuId, estado);
        }

        // Actualiza el perfil del usuario (nombre, cedula, celular)
        public void ActualizarPerfil(int usuId, string nombre, string cedula, string celular)
        {
            usuarioDatos.ActualizarPerfil(usuId, nombre, cedula, celular);
        }

        // Actualiza la ruta de la imagen de perfil
        public void ActualizarImagenPerfil(int usuId, string rutaImagen)
        {
            usuarioDatos.ActualizarImagenPerfil(usuId, rutaImagen);
        }
    }
}