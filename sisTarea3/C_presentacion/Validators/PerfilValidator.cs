using System;
using FluentValidation;

namespace C_presentacion.Validators
{
    // Valida los campos del formulario MiPerfil usando FluentValidation
    public class PerfilValidator : AbstractValidator<PerfilViewModel>
    {
        public PerfilValidator()
        {
            // Nombre: obligatorio, minimo 2 caracteres
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.");

            // Cedula: opcional, solo digitos si se ingresa
            RuleFor(x => x.Cedula)
                .Matches(@"^\d*$").When(x => !string.IsNullOrEmpty(x.Cedula))
                .WithMessage("La cedula solo debe contener numeros.");

            // Celular: opcional, solo digitos si se ingresa
            RuleFor(x => x.Celular)
                .Matches(@"^\d*$").When(x => !string.IsNullOrEmpty(x.Celular))
                .WithMessage("El celular solo debe contener numeros.");

            // Fecha de nacimiento: si se ingresa, entre 12 y 100 años
            RuleFor(x => x.FechaNacimiento)
                .Must(EdadValida).When(x => x.FechaNacimiento.HasValue)
                .WithMessage("La fecha debe corresponder a una edad entre 12 y 100 años.");
        }

        // Valida que la edad este entre 12 y 100 años
        private bool EdadValida(DateTime? fecha)
        {
            if (!fecha.HasValue) return true;
            var hoy = DateTime.Today;
            var edad = hoy.Year - fecha.Value.Year;
            if (fecha.Value.Date > hoy.AddYears(-edad)) edad--;
            return edad >= 12 && edad <= 100;
        }
    }

    // Modelo simple con los datos del formulario de perfil
    public class PerfilViewModel
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Celular { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
