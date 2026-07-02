using System;
using System.Data;

namespace C_negocio
{
    public class UsuarioNegocio
    {
        private C_datos.UsuarioDatos usuarioDatos = new C_datos.UsuarioDatos();

        public DataTable Login(string correo, string contrasena)
        {
            return usuarioDatos.ValidarUsuario(correo, contrasena);
        }

        public int Registrar(string nombre, string correo, string contrasena, string cedula, string celular, string rol, DateTime? fechaNacimiento = null, string correoSecundario = null)
        {
            return usuarioDatos.RegistrarUsuario(nombre, correo, contrasena, cedula, celular, rol, fechaNacimiento, correoSecundario);
        }

        public bool CedulaExiste(string cedula)
        {
            return usuarioDatos.CedulaExiste(cedula);
        }

        public bool CedulaExiste(string cedula, int excludeUserId)
        {
            return usuarioDatos.CedulaExiste(cedula, excludeUserId);
        }

        public bool CorreoExiste(string correo)
        {
            return usuarioDatos.CorreoExiste(correo);
        }

        public DataTable ObtenerUsuarioPorCorreo(string correo)
        {
            return usuarioDatos.ObtenerUsuarioPorCorreo(correo);
        }

        public string RecuperarContrasena(string correo)
        {
            return usuarioDatos.ObtenerContrasenaPorCorreo(correo);
        }

        public void ManejarIntentoFallido(int usuId, int intentosActuales)
        {
            int nuevosIntentos = intentosActuales + 1;
            if (nuevosIntentos >= 3)
            {
                usuarioDatos.BloquearUsuario(usuId);
            }
            usuarioDatos.ActualizarIntentos(usuId, nuevosIntentos);
        }

        public void ReiniciarIntentos(int usuId)
        {
            usuarioDatos.ActualizarIntentos(usuId, 0);
        }

        public DataTable ListarUsuarios()
        {
            return usuarioDatos.ListarUsuarios();
        }

        public DataTable ObtenerUsuarioPorId(int usuId)
        {
            return usuarioDatos.ObtenerUsuarioPorId(usuId);
        }

        public void ActualizarUsuario(int usuId, string nombre, string correo, string cedula, string celular, string rol, DateTime? fechaNacimiento = null, string correoSecundario = null)
        {
            usuarioDatos.ActualizarUsuario(usuId, nombre, correo, cedula, celular, rol, fechaNacimiento, correoSecundario);
        }

        public void CambiarEstadoUsuario(int usuId, string estado)
        {
            usuarioDatos.CambiarEstadoUsuario(usuId, estado);
        }

        public void ActualizarPerfil(int usuId, string nombre, string cedula, string celular, DateTime? fechaNacimiento = null, string correoSecundario = null)
        {
            usuarioDatos.ActualizarPerfil(usuId, nombre, cedula, celular, fechaNacimiento, correoSecundario);
        }

        public void CambiarContrasena(int usuId, string nuevaContrasena)
        {
            usuarioDatos.ActualizarContrasena(usuId, nuevaContrasena);
        }

        public static string CalcularEdad(DateTime fechaNacimiento)
        {
            DateTime hoy = DateTime.Today;
            int años = hoy.Year - fechaNacimiento.Year;
            int meses = hoy.Month - fechaNacimiento.Month;
            int dias = hoy.Day - fechaNacimiento.Day;

            if (dias < 0)
            {
                meses--;
                dias += DateTime.DaysInMonth(hoy.Year, hoy.Month == 1 ? 12 : hoy.Month - 1);
            }
            if (meses < 0)
            {
                años--;
                meses += 12;
            }

            return $"{años} años, {meses} meses, {dias} días";
        }

        public static string ObtenerSignoZodiacal(DateTime fecha)
        {
            int m = fecha.Month, d = fecha.Day;
            if ((m == 3 && d >= 21) || (m == 4 && d <= 19)) return "\u2648 Aries";
            if ((m == 4 && d >= 20) || (m == 5 && d <= 20)) return "\u2649 Tauro";
            if ((m == 5 && d >= 21) || (m == 6 && d <= 20)) return "\u264A Géminis";
            if ((m == 6 && d >= 21) || (m == 7 && d <= 22)) return "\u264B Cáncer";
            if ((m == 7 && d >= 23) || (m == 8 && d <= 22)) return "\u264C Leo";
            if ((m == 8 && d >= 23) || (m == 9 && d <= 22)) return "\u264D Virgo";
            if ((m == 9 && d >= 23) || (m == 10 && d <= 22)) return "\u264E Libra";
            if ((m == 10 && d >= 23) || (m == 11 && d <= 21)) return "\u264F Escorpio";
            if ((m == 11 && d >= 22) || (m == 12 && d <= 21)) return "\u2650 Sagitario";
            if ((m == 12 && d >= 22) || (m == 1 && d <= 19)) return "\u2651 Capricornio";
            if ((m == 1 && d >= 20) || (m == 2 && d <= 18)) return "\u2652 Acuario";
            return "\u2653 Piscis";
        }
    }
}
