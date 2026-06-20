using CapaDatos.Seguridad;
using CapaEntidad.Seguridad;
using CapaNegocio.Recursos;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Seguridad
{
    public class CN_Seguridad
    {
        private CD_Seguridad objCapaDato = new CD_Seguridad();

        public List<ModuloSistema> ListarModulos()
        {
            return objCapaDato.ListarModulos();
        }

        public List<PermisoRolModulo> ListarPermisosPorRol(int idRol)
        {
            if (idRol <= 0)
            {
                return new List<PermisoRolModulo>();
            }

            return objCapaDato.ListarPermisosPorRol(idRol);
        }

        public bool GuardarPermisoRolModulo(PermisoRolModulo obj, string identificacionEmpleado, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararPermiso(obj);

            string error = ValidarPermiso(obj, identificacionEmpleado);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.GuardarPermisoRolModulo(obj, identificacionEmpleado, out Mensaje);
        }

        public bool InactivarPermisoRolModulo(int idRol, string codigoModulo, string identificacionEmpleado, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoModulo = codigoModulo == null ? "" : codigoModulo.Trim();
            identificacionEmpleado = identificacionEmpleado == null ? "" : identificacionEmpleado.Trim();

            if (idRol <= 0)
            {
                Mensaje = "Debe seleccionar un rol válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(codigoModulo))
            {
                Mensaje = "Debe seleccionar un módulo.";
                return false;
            }

            if (codigoModulo.Length > 80)
            {
                Mensaje = "El código del módulo no puede superar los 80 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(codigoModulo, @"^[A-Z0-9_]+$"))
            {
                Mensaje = "El código del módulo contiene caracteres no permitidos.";
                return false;
            }

            if (!ValidarIdentificacionEmpleado(identificacionEmpleado))
            {
                Mensaje = "La identificación del empleado que realiza el cambio no es válida.";
                return false;
            }

            return objCapaDato.InactivarPermisoRolModulo(idRol, codigoModulo, identificacionEmpleado, out Mensaje);
        }

        public bool InactivarModuloSistema(string codigoModulo, string identificacionEmpleado, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoModulo = codigoModulo == null ? "" : codigoModulo.Trim();
            identificacionEmpleado = identificacionEmpleado == null ? "" : identificacionEmpleado.Trim();

            if (string.IsNullOrWhiteSpace(codigoModulo))
            {
                Mensaje = "Debe seleccionar un módulo.";
                return false;
            }

            if (codigoModulo.Length > 80)
            {
                Mensaje = "El código del módulo no puede superar los 80 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(codigoModulo, @"^[A-Z0-9_]+$"))
            {
                Mensaje = "El código del módulo contiene caracteres no permitidos.";
                return false;
            }

            if (!ValidarIdentificacionEmpleado(identificacionEmpleado))
            {
                Mensaje = "La identificación del empleado que realiza el cambio no es válida.";
                return false;
            }

            return objCapaDato.InactivarModuloSistema(codigoModulo, identificacionEmpleado, out Mensaje);
        }

        public List<PermisoRolModulo> ObtenerPermisosPorRol(int idRol)
        {
            if (idRol <= 0)
            {
                return new List<PermisoRolModulo>();
            }

            return objCapaDato.ObtenerPermisosPorRol(idRol);
        }

        public bool ValidarAcceso(int idRol, string controlador, string accion)
        {
            controlador = controlador == null ? "" : controlador.Trim();
            accion = accion == null ? "" : accion.Trim();

            if (idRol <= 0)
            {
                return false;
            }

            if (controlador == "" || accion == "")
            {
                return false;
            }

            if (controlador.Length > 100 || accion.Length > 100)
            {
                return false;
            }

            if (!Regex.IsMatch(controlador, @"^[a-zA-Z0-9_]+$"))
            {
                return false;
            }

            if (!Regex.IsMatch(accion, @"^[a-zA-Z0-9_]+$"))
            {
                return false;
            }

            return objCapaDato.ValidarAcceso(idRol, controlador, accion);
        }

        public bool RegistrarHistorialCambio(
            string identificacion,
            string nombreTabla,
            string tipoDeObservacion,
            string detalle,
            string valorAnterior,
            string valorNuevo,
            string idRegistro,
            out string Mensaje
        )
        {
            Mensaje = string.Empty;

            identificacion = identificacion == null ? "" : identificacion.Trim();
            nombreTabla = nombreTabla == null ? "" : nombreTabla.Trim();
            tipoDeObservacion = tipoDeObservacion == null ? "" : tipoDeObservacion.Trim();
            detalle = detalle == null ? "" : detalle.Trim();
            valorAnterior = valorAnterior == null ? "" : valorAnterior.Trim();
            valorNuevo = valorNuevo == null ? "" : valorNuevo.Trim();
            idRegistro = idRegistro == null ? "" : idRegistro.Trim();

            if (!ValidarIdentificacionEmpleado(identificacion))
            {
                Mensaje = "La identificación del empleado no es válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombreTabla))
            {
                Mensaje = "Debe indicar la tabla afectada.";
                return false;
            }

            if (nombreTabla.Length > 100)
            {
                Mensaje = "El nombre de la tabla no puede superar los 100 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(nombreTabla, @"^[a-zA-Z0-9_]+$"))
            {
                Mensaje = "El nombre de la tabla contiene caracteres no permitidos.";
                return false;
            }

            if (tipoDeObservacion.Length > 45)
            {
                Mensaje = "El tipo de observación no puede superar los 45 caracteres.";
                return false;
            }

            if (detalle.Length > 300)
            {
                Mensaje = "El detalle no puede superar los 300 caracteres.";
                return false;
            }

            if (valorAnterior.Length > 900)
            {
                valorAnterior = valorAnterior.Substring(0, 900);
            }

            if (valorNuevo.Length > 900)
            {
                valorNuevo = valorNuevo.Substring(0, 900);
            }

            if (idRegistro.Length > 150)
            {
                idRegistro = idRegistro.Substring(0, 150);
            }

            return objCapaDato.RegistrarHistorialCambio(
                identificacion,
                nombreTabla,
                tipoDeObservacion,
                detalle,
                valorAnterior,
                valorNuevo,
                idRegistro,
                out Mensaje
            );
        }

        public UsuarioSesion IniciarSesion(string nombreUsuario, string clave, out string Mensaje)
        {
            Mensaje = string.Empty;

            nombreUsuario = nombreUsuario == null ? "" : nombreUsuario.Trim();
            clave = clave == null ? "" : clave.Trim();

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                Mensaje = "Debe ingresar el nombre de usuario.";
                return null;
            }

            if (string.IsNullOrWhiteSpace(clave))
            {
                Mensaje = "Debe ingresar la contraseña.";
                return null;
            }

            if (nombreUsuario.Length > 45)
            {
                Mensaje = "El nombre de usuario no puede superar los 45 caracteres.";
                return null;
            }

            if (clave.Length > 100)
            {
                Mensaje = "La contraseña no puede superar los 100 caracteres.";
                return null;
            }

            string claveHash = CN_Recursos.ConvertirSha256(clave);

            UsuarioSesion usuario = objCapaDato.IniciarSesion(nombreUsuario, claveHash);

            if (usuario == null)
            {
                Mensaje = "Usuario o contraseña incorrectos.";
                return null;
            }

            usuario.Permisos = objCapaDato.ObtenerPermisosPorRol(usuario.IdRol);

            objCapaDato.ActualizarUltimoAcceso(usuario.Identificacion);

            string mensajeEvento;
            objCapaDato.RegistrarEventoSesion(
                usuario.Identificacion,
                "Inicio de sesión",
                out mensajeEvento
            );

            return usuario;
        }

        public bool CambiarClavePropia(string identificacion, string claveActual, string claveNueva, string confirmarClave, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacion = identificacion == null ? "" : identificacion.Trim();
            claveActual = claveActual == null ? "" : claveActual.Trim();
            claveNueva = claveNueva == null ? "" : claveNueva.Trim();
            confirmarClave = confirmarClave == null ? "" : confirmarClave.Trim();

            if (!ValidarIdentificacionEmpleado(identificacion))
            {
                Mensaje = "No se pudo identificar al usuario de la sesión.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(claveActual))
            {
                Mensaje = "Debe ingresar la clave actual.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(claveNueva))
            {
                Mensaje = "Debe ingresar la nueva clave.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(confirmarClave))
            {
                Mensaje = "Debe confirmar la nueva clave.";
                return false;
            }

            if (claveNueva != confirmarClave)
            {
                Mensaje = "La nueva clave y la confirmación no coinciden.";
                return false;
            }

            if (claveNueva.Length < 8)
            {
                Mensaje = "La nueva clave debe tener al menos 8 caracteres.";
                return false;
            }

            if (claveNueva.Length > 100)
            {
                Mensaje = "La nueva clave no puede superar los 100 caracteres.";
                return false;
            }

            if (claveActual == claveNueva)
            {
                Mensaje = "La nueva clave no puede ser igual a la clave actual.";
                return false;
            }

            bool tieneMayuscula = Regex.IsMatch(claveNueva, @"[A-Z]");
            bool tieneMinuscula = Regex.IsMatch(claveNueva, @"[a-z]");
            bool tieneNumero = Regex.IsMatch(claveNueva, @"[0-9]");

            if (!tieneMayuscula || !tieneMinuscula || !tieneNumero)
            {
                Mensaje = "La nueva clave debe incluir mayúscula, minúscula y número.";
                return false;
            }

            string claveActualHash = CN_Recursos.ConvertirSha256(claveActual);
            string claveNuevaHash = CN_Recursos.ConvertirSha256(claveNueva);

            return objCapaDato.CambiarClavePropia(
                identificacion,
                claveActualHash,
                claveNuevaHash,
                out Mensaje
            );
        }

        public bool RegistrarCierreSesion(string identificacion)
        {
            string mensaje;

            return objCapaDato.RegistrarEventoSesion(
                identificacion,
                "Cierre de sesión",
                out mensaje
            );

        }
        private void PrepararPermiso(PermisoRolModulo obj)
        {
            obj.CodigoModulo = obj.CodigoModulo == null ? "" : obj.CodigoModulo.Trim();
            obj.NombreModulo = obj.NombreModulo == null ? "" : obj.NombreModulo.Trim();
            obj.AreaSistema = obj.AreaSistema == null ? "" : obj.AreaSistema.Trim();
            obj.Controlador = obj.Controlador == null ? "" : obj.Controlador.Trim();
            obj.Accion = obj.Accion == null ? "" : obj.Accion.Trim();
            obj.Url = obj.Url == null ? "" : obj.Url.Trim();
            obj.Icono = obj.Icono == null ? "" : obj.Icono.Trim();

            if (!obj.PuedeVer)
            {
                obj.PuedeCrear = false;
                obj.PuedeEditar = false;
                obj.PuedeEliminar = false;
            }
        }

        private string ValidarPermiso(PermisoRolModulo obj, string identificacionEmpleado)
        {
            identificacionEmpleado = identificacionEmpleado == null ? "" : identificacionEmpleado.Trim();

            if (obj.IdRol <= 0)
            {
                return "Debe seleccionar un rol válido.";
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoModulo))
            {
                return "Debe seleccionar un módulo.";
            }

            if (obj.CodigoModulo.Length > 80)
            {
                return "El código del módulo no puede superar los 80 caracteres.";
            }

            if (!Regex.IsMatch(obj.CodigoModulo, @"^[A-Z0-9_]+$"))
            {
                return "El código del módulo contiene caracteres no permitidos.";
            }

            if (!ValidarIdentificacionEmpleado(identificacionEmpleado))
            {
                return "La identificación del empleado que realiza el cambio no es válida.";
            }

            return "";
        }

        private bool ValidarIdentificacionEmpleado(string identificacion)
        {
            identificacion = identificacion == null ? "" : identificacion.Trim();

            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return false;
            }

            if (identificacion.Length > 45)
            {
                return false;
            }

            return Regex.IsMatch(identificacion, @"^[a-zA-Z0-9\-]+$");
        }

        public List<HistorialCambioConsulta> ListarHistorialCambios(string filtro, string fechaInicioTexto, string fechaFinTexto, string tipoDeObservacion, out string Mensaje)
        {
            Mensaje = string.Empty;

            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            filtro = filtro == null ? "" : filtro.Trim();
            fechaInicioTexto = fechaInicioTexto == null ? "" : fechaInicioTexto.Trim();
            fechaFinTexto = fechaFinTexto == null ? "" : fechaFinTexto.Trim();
            tipoDeObservacion = tipoDeObservacion == null ? "" : tipoDeObservacion.Trim();

            if (filtro.Length > 100)
            {
                Mensaje = "El filtro no puede superar los 100 caracteres.";
                return new List<HistorialCambioConsulta>();
            }

            if (tipoDeObservacion.Length > 45)
            {
                Mensaje = "El tipo de observación no puede superar los 45 caracteres.";
                return new List<HistorialCambioConsulta>();
            }

            if (fechaInicioTexto != "")
            {
                DateTime fechaTemp;

                if (!DateTime.TryParse(fechaInicioTexto, out fechaTemp))
                {
                    Mensaje = "La fecha de inicio no tiene un formato válido.";
                    return new List<HistorialCambioConsulta>();
                }

                fechaInicio = fechaTemp.Date;
            }

            if (fechaFinTexto != "")
            {
                DateTime fechaTemp;

                if (!DateTime.TryParse(fechaFinTexto, out fechaTemp))
                {
                    Mensaje = "La fecha final no tiene un formato válido.";
                    return new List<HistorialCambioConsulta>();
                }

                fechaFin = fechaTemp.Date;
            }

            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio.Value > fechaFin.Value)
            {
                Mensaje = "La fecha de inicio no puede ser mayor que la fecha final.";
                return new List<HistorialCambioConsulta>();
            }

            return objCapaDato.ListarHistorialCambios(
                filtro,
                fechaInicio,
                fechaFin,
                tipoDeObservacion
            );
        }
    }
}