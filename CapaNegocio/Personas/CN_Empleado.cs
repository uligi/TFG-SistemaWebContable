using CapaDatos.personas;
using CapaEntidad.personas;
using CapaNegocio.Recursos;
using System.Collections.Generic;

namespace CapaNegocio.personas
{
    public class CN_Empleado
    {
        private CD_Empleado objCapaDato = new CD_Empleado();

        public List<Empleado> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Empleado obj, out string Mensaje, out string claveTemporal, out string nombreUsuario, out string codigoEmpleado)
        {
            Mensaje = string.Empty;
            claveTemporal = string.Empty;
            nombreUsuario = string.Empty;
            codigoEmpleado = string.Empty;

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

            if (obj.IdPuesto.HasValue && obj.IdPuesto.Value < 0)
            {
                Mensaje = "Debe seleccionar un puesto válido.";
                return 0;
            }

            if (obj.IdRol == 0)
            {
                Mensaje = "Debe seleccionar un rol.";
                return 0;
            }

            claveTemporal = CN_Recursos.GenerarClaveTemporal();
            obj.ClaveHash = CN_Recursos.ConvertirSha256(claveTemporal);

            int resultado = objCapaDato.Registrar(obj, out Mensaje, out nombreUsuario, out codigoEmpleado);

            if (resultado == 0)
            {
                claveTemporal = string.Empty;
                nombreUsuario = string.Empty;
                codigoEmpleado = string.Empty;
            }

            return resultado;
        }

        public bool Editar(Empleado obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "Debe seleccionar un empleado válido.";
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

            if (obj.IdPuesto.HasValue && obj.IdPuesto.Value < 0)
            {
                Mensaje = "Debe seleccionar un puesto válido.";
                return false;
            }

            if (obj.IdRol == 0)
            {
                Mensaje = "Debe seleccionar un rol.";
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

            return objCapaDato.Inactivar(identificacion, out Mensaje);
        }

        public bool RestablecerClave(string identificacion, out string Mensaje, out string claveTemporal, out string nombreUsuario)
        {
            Mensaje = string.Empty;
            claveTemporal = string.Empty;
            nombreUsuario = string.Empty;

            if (string.IsNullOrWhiteSpace(identificacion))
            {
                Mensaje = "Debe seleccionar un empleado válido.";
                return false;
            }

            claveTemporal = CN_Recursos.GenerarClaveTemporal();
            string claveHash = CN_Recursos.ConvertirSha256(claveTemporal);

            bool resultado = objCapaDato.RestablecerClave(identificacion, claveHash, out Mensaje, out nombreUsuario);

            if (!resultado)
            {
                claveTemporal = string.Empty;
                nombreUsuario = string.Empty;
            }

            return resultado;
        }
    }
}