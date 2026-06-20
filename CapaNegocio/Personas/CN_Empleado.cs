using CapaDatos.personas;
using CapaEntidad.Personas;
using CapaNegocio.Recursos;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.personas
{
    public class CN_Empleado
    {
        private CD_Empleado objCapaDato = new CD_Empleado();

        public List<Empleado> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(
            Empleado obj,
            out string Mensaje,
            out string NombreUsuarioGenerado,
            out string CodigoEmpleadoGenerado,
            out string ClaveTemporalGenerada
        )
        {
            Mensaje = string.Empty;
            NombreUsuarioGenerado = string.Empty;
            CodigoEmpleadoGenerado = string.Empty;
            ClaveTemporalGenerada = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            obj.Clave = CN_Recursos.GenerarClaveTemporal();
            obj.ClaveHash = CN_Recursos.ConvertirSha256(obj.Clave);
            ClaveTemporalGenerada = obj.Clave;

            int resultado = objCapaDato.Registrar(
                obj,
                out Mensaje,
                out NombreUsuarioGenerado,
                out CodigoEmpleadoGenerado
            );

            if (resultado == 0)
            {
                ClaveTemporalGenerada = string.Empty;
            }

            return resultado;
        }

        public bool Editar(Empleado obj, out string Mensaje)
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
                Mensaje = "Debe seleccionar un empleado válido.";
                return false;
            }

            identificacion = identificacion.Trim();

            if (!Regex.IsMatch(identificacion, @"^([0-9]{9}|[0-9]{11})$"))
            {
                Mensaje = "La identificación debe contener solo números: 9 dígitos para cédula nacional o 11 dígitos para cédula extranjera.";
                return false;
            }

            return objCapaDato.Inactivar(identificacion, out Mensaje);
        }

        public bool RestablecerClave(
            string identificacion,
            out string Mensaje,
            out string NombreUsuario,
            out string ClaveTemporalGenerada
        )
        {
            Mensaje = string.Empty;
            NombreUsuario = string.Empty;
            ClaveTemporalGenerada = string.Empty;

            if (string.IsNullOrWhiteSpace(identificacion))
            {
                Mensaje = "Debe seleccionar un empleado válido.";
                return false;
            }

            identificacion = identificacion.Trim();

            if (!Regex.IsMatch(identificacion, @"^([0-9]{9}|[0-9]{11})$"))
            {
                Mensaje = "La identificación debe contener solo números: 9 dígitos para cédula nacional o 11 dígitos para cédula extranjera.";
                return false;
            }

            ClaveTemporalGenerada = CN_Recursos.GenerarClaveTemporal();
            string claveHash = CN_Recursos.ConvertirSha256(ClaveTemporalGenerada);

            bool resultado = objCapaDato.RestablecerClave(
                identificacion,
                claveHash,
                out Mensaje,
                out NombreUsuario
            );

            if (!resultado)
            {
                ClaveTemporalGenerada = string.Empty;
            }

            return resultado;
        }

        private void PrepararDatos(Empleado obj)
        {
            obj.Identificacion = obj.Identificacion == null ? "" : obj.Identificacion.Trim();
            obj.Nombre = obj.Nombre == null ? "" : obj.Nombre.Trim();
            obj.PrimerApellido = obj.PrimerApellido == null ? "" : obj.PrimerApellido.Trim();
            obj.SegundoApellido = obj.SegundoApellido == null ? "" : obj.SegundoApellido.Trim();
            obj.Direccion = obj.Direccion == null ? "" : obj.Direccion.Trim();
            obj.Clave = obj.Clave == null ? "" : obj.Clave.Trim();
            obj.ClaveHash = obj.ClaveHash == null ? "" : obj.ClaveHash.Trim();
            obj.CodigoEmpleado = obj.CodigoEmpleado == null ? "" : obj.CodigoEmpleado.Trim();
            obj.NombreUsuario = obj.NombreUsuario == null ? "" : obj.NombreUsuario.Trim();
        }

        private string Validar(Empleado obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                return esEdicion ? "Debe seleccionar un empleado válido." : "La identificación es obligatoria.";
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
                return "Solo se pueden registrar empleados mayores de edad.";
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

            if (!Regex.IsMatch(obj.Direccion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La dirección solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.IdPuesto <= 0)
            {
                return "Debe seleccionar un puesto.";
            }

            if (!obj.FechaIngreso.HasValue)
            {
                return "La fecha de ingreso es obligatoria.";
            }

            if (obj.FechaIngreso.Value.Date < fechaNacimiento)
            {
                return "La fecha de ingreso no puede ser menor que la fecha de nacimiento.";
            }

            if (obj.IdRol <= 0)
            {
                return "Debe seleccionar un rol.";
            }

            return "";
        }
    }
}