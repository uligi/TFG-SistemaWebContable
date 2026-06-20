using CapaDatos.personas;
using CapaEntidad.Personas;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

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

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, true);

            if (error != "")
            {
                Mensaje = error;
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

            return objCapaDato.Inactivar(identificacion.Trim(), out Mensaje);
        }

        private void PrepararDatos(Cliente obj)
        {
            obj.Identificacion = obj.Identificacion == null ? "" : obj.Identificacion.Trim();
            obj.Nombre = obj.Nombre == null ? "" : obj.Nombre.Trim();
            obj.PrimerApellido = obj.PrimerApellido == null ? "" : obj.PrimerApellido.Trim();
            obj.SegundoApellido = obj.SegundoApellido == null ? "" : obj.SegundoApellido.Trim();
            obj.Direccion = obj.Direccion == null ? "" : obj.Direccion.Trim();

            if (obj.LimiteCredito < 0)
            {
                obj.LimiteCredito = 0;
            }

            if (obj.DiasCredito < 0)
            {
                obj.DiasCredito = 0;
            }
        }

        private string Validar(Cliente obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                return esEdicion ? "Debe seleccionar un cliente válido." : "La identificación es obligatoria.";
            }

            if (!Regex.IsMatch(obj.Identificacion, @"^([0-9]{9}|[0-9]{11})$"))
            {
                return "La identificación debe contener solo números: 9 dígitos para cédula nacional o 11 dígitos para cédula extranjera.";
            }

            if (!obj.FechaNacimiento.HasValue)
            {
                return "La fecha de nacimiento es obligatoria.";
            }

            DateTime fechaNacimiento = obj.FechaNacimiento.Value.Date;
            DateTime fechaActual = DateTime.Today;

            if (fechaNacimiento >= fechaActual)
            {
                return "La fecha de nacimiento no puede ser igual o mayor a la fecha actual.";
            }

            if (fechaNacimiento > fechaActual.AddYears(-18))
            {
                return "Solo se pueden registrar personas mayores de edad.";
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (obj.Nombre.Length > 45)
            {
                return "El nombre no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"))
            {
                return "El nombre solo puede contener letras y espacios.";
            }

            if (string.IsNullOrWhiteSpace(obj.PrimerApellido))
            {
                return "El primer apellido es obligatorio.";
            }

            if (obj.PrimerApellido.Length > 45)
            {
                return "El primer apellido no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.PrimerApellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"))
            {
                return "El primer apellido solo puede contener letras y espacios.";
            }

            if (!string.IsNullOrWhiteSpace(obj.SegundoApellido))
            {
                if (obj.SegundoApellido.Length > 45)
                {
                    return "El segundo apellido no puede superar los 45 caracteres.";
                }

                if (!Regex.IsMatch(obj.SegundoApellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"))
                {
                    return "El segundo apellido solo puede contener letras y espacios.";
                }
            }

            if (obj.CodigoDistrito <= 0)
            {
                return "Debe seleccionar un distrito.";
            }

            if (string.IsNullOrWhiteSpace(obj.Direccion))
            {
                return "La dirección exacta es obligatoria.";
            }

            if (obj.Direccion.Length > 250)
            {
                return "La dirección no puede superar los 250 caracteres.";
            }

            if (obj.LimiteCredito < 0)
            {
                return "El límite de crédito no puede ser negativo.";
            }

            if (obj.DiasCredito < 0)
            {
                return "Los días de crédito no pueden ser negativos.";
            }

            return "";
        }
    }
}