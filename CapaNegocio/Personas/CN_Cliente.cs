using CapaDatos.personas;

using CapaEntidad.Personas;
using System.Collections.Generic;

namespace CapaNegocio.personas
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDato = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "La identificación es obligatoria.";
                return 0;
            }

            if (obj.Identificacion.Length > 45)
            {
                Mensaje = "La identificación no puede superar los 45 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre es obligatorio.";
                return 0;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre no puede superar los 45 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.PrimerApellido))
            {
                Mensaje = "El primer apellido es obligatorio.";
                return 0;
            }

            if (obj.PrimerApellido.Length > 45)
            {
                Mensaje = "El primer apellido no puede superar los 45 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.SegundoApellido) && obj.SegundoApellido.Length > 45)
            {
                Mensaje = "El segundo apellido no puede superar los 45 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.Direccion) && obj.Direccion.Length > 250)
            {
                Mensaje = "La dirección no puede superar los 250 caracteres.";
                return 0;
            }

            if (obj.LimiteCredito.HasValue && obj.LimiteCredito.Value < 0)
            {
                Mensaje = "El límite de crédito no puede ser negativo.";
                return 0;
            }

            if (obj.DiasCredito.HasValue && obj.DiasCredito.Value < 0)
            {
                Mensaje = "Los días de crédito no pueden ser negativos.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "Debe seleccionar un cliente válido.";
                return false;
            }

            if (obj.Identificacion.Length > 45)
            {
                Mensaje = "La identificación no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre es obligatorio.";
                return false;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.PrimerApellido))
            {
                Mensaje = "El primer apellido es obligatorio.";
                return false;
            }

            if (obj.PrimerApellido.Length > 45)
            {
                Mensaje = "El primer apellido no puede superar los 45 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.SegundoApellido) && obj.SegundoApellido.Length > 45)
            {
                Mensaje = "El segundo apellido no puede superar los 45 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.Direccion) && obj.Direccion.Length > 250)
            {
                Mensaje = "La dirección no puede superar los 250 caracteres.";
                return false;
            }

            if (obj.LimiteCredito.HasValue && obj.LimiteCredito.Value < 0)
            {
                Mensaje = "El límite de crédito no puede ser negativo.";
                return false;
            }

            if (obj.DiasCredito.HasValue && obj.DiasCredito.Value < 0)
            {
                Mensaje = "Los días de crédito no pueden ser negativos.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(string identificacion, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(identificacion))
            {
                Mensaje = "Debe seleccionar un cliente válido.";
                return false;
            }

            return objCapaDato.Inactivar(identificacion, out Mensaje);
        }
    }
}