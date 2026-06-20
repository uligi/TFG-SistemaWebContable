using CapaEntidad.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Seguridad
{
    public class CD_Seguridad
    {
        public List<ModuloSistema> ListarModulos()
        {
            List<ModuloSistema> lista = new List<ModuloSistema>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_ModuloSistema_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ModuloSistema()
                            {
                                CodigoModulo = dr["CodigoModulo"].ToString(),
                                NombreModulo = dr["NombreModulo"].ToString(),
                                AreaSistema = dr["AreaSistema"].ToString(),
                                Controlador = dr["Controlador"].ToString(),
                                Accion = dr["Accion"].ToString(),
                                Url = dr["Url"].ToString(),
                                Icono = dr["Icono"].ToString(),
                                Orden = Convert.ToInt32(dr["Orden"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ModuloSistema>();
            }

            return lista;
        }

        public List<PermisoRolModulo> ListarPermisosPorRol(int idRol)
        {
            List<PermisoRolModulo> lista = new List<PermisoRolModulo>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_PermisoRolModulo_ListarPorRol", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearPermisoRolModulo(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PermisoRolModulo>();
            }

            return lista;
        }

        public bool GuardarPermisoRolModulo(PermisoRolModulo obj, string identificacionEmpleado, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_PermisoRolModulo_Guardar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdRol", obj.IdRol);
                    cmd.Parameters.AddWithValue("@CodigoModulo", obj.CodigoModulo ?? "");
                    cmd.Parameters.AddWithValue("@PuedeVer", obj.PuedeVer);
                    cmd.Parameters.AddWithValue("@PuedeCrear", obj.PuedeCrear);
                    cmd.Parameters.AddWithValue("@PuedeEditar", obj.PuedeEditar);
                    cmd.Parameters.AddWithValue("@PuedeEliminar", obj.PuedeEliminar);
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", identificacionEmpleado ?? "");

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool InactivarPermisoRolModulo(int idRol, string codigoModulo, string identificacionEmpleado, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_PermisoRolModulo_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.Parameters.AddWithValue("@CodigoModulo", codigoModulo ?? "");
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", identificacionEmpleado ?? "");

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool InactivarModuloSistema(string codigoModulo, string identificacionEmpleado, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_ModuloSistema_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoModulo", codigoModulo ?? "");
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", identificacionEmpleado ?? "");

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public List<PermisoRolModulo> ObtenerPermisosPorRol(int idRol)
        {
            List<PermisoRolModulo> lista = new List<PermisoRolModulo>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_PermisoRolModulo_ObtenerPorRol", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdRol", idRol);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearPermisoRolModulo(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PermisoRolModulo>();
            }

            return lista;
        }

        public bool ValidarAcceso(int idRol, string controlador, string accion)
        {
            bool tieneAcceso = false;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_PermisoRolModulo_ValidarAcceso", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.Parameters.AddWithValue("@Controlador", controlador ?? "");
                    cmd.Parameters.AddWithValue("@Accion", accion ?? "");

                    oconexion.Open();

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null && resultado != DBNull.Value)
                    {
                        tieneAcceso = Convert.ToBoolean(resultado);
                    }
                }
            }
            catch
            {
                tieneAcceso = false;
            }

            return tieneAcceso;
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
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_HistorialCambio_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");
                    cmd.Parameters.AddWithValue("@NombreTabla", nombreTabla ?? "");
                    cmd.Parameters.AddWithValue("@TipoDeObservacion", tipoDeObservacion ?? "");
                    cmd.Parameters.AddWithValue("@Detalle", detalle ?? "");
                    cmd.Parameters.AddWithValue("@ValorAnterior", valorAnterior ?? "");
                    cmd.Parameters.AddWithValue("@ValorNuevo", valorNuevo ?? "");
                    cmd.Parameters.AddWithValue("@IdRegistro", idRegistro ?? "");

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        private PermisoRolModulo MapearPermisoRolModulo(SqlDataReader dr)
        {
            PermisoRolModulo obj = new PermisoRolModulo()
            {
                IdRol = Convert.ToInt32(dr["IdRol"]),
                CodigoModulo = dr["CodigoModulo"].ToString(),
                NombreModulo = dr["NombreModulo"].ToString(),
                AreaSistema = dr["AreaSistema"].ToString(),
                Controlador = dr["Controlador"].ToString(),
                Accion = dr["Accion"].ToString(),
                Url = dr["Url"].ToString(),
                Icono = dr["Icono"].ToString(),
                Orden = Convert.ToInt32(dr["Orden"]),

                PuedeVer = Convert.ToBoolean(dr["PuedeVer"]),
                PuedeCrear = Convert.ToBoolean(dr["PuedeCrear"]),
                PuedeEditar = Convert.ToBoolean(dr["PuedeEditar"]),
                PuedeEliminar = Convert.ToBoolean(dr["PuedeEliminar"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };

            return obj;
        }


        public UsuarioSesion IniciarSesion(string nombreUsuario, string claveHash)
        {
            UsuarioSesion obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_Acceso_IniciarSesion", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario ?? "");
                    cmd.Parameters.AddWithValue("@ClaveHash", claveHash ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = new UsuarioSesion()
                            {
                                Identificacion = dr["Identificacion"].ToString(),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                NombreRol = dr["NombreRol"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                RestablecerClave = Convert.ToBoolean(dr["RestablecerClave"])
                            };
                        }
                    }
                }
            }
            catch
            {
                obj = null;
            }

            return obj;
        }

        public void ActualizarUltimoAcceso(string identificacion)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_Acceso_ActualizarUltimoAcceso", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }

        public bool RegistrarEventoSesion(string identificacion, string evento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_Acceso_RegistrarEventoSesion", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");
                    cmd.Parameters.AddWithValue("@Evento", evento ?? "");

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool CambiarClavePropia(string identificacion, string claveActualHash, string claveNuevaHash, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_Acceso_CambiarClavePropia", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");
                    cmd.Parameters.AddWithValue("@ClaveActualHash", claveActualHash ?? "");
                    cmd.Parameters.AddWithValue("@ClaveNuevaHash", claveNuevaHash ?? "");

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public List<HistorialCambioConsulta> ListarHistorialCambios(string filtro, DateTime? fechaInicio, DateTime? fechaFin, string tipoDeObservacion)
        {
            List<HistorialCambioConsulta> lista = new List<HistorialCambioConsulta>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Seguridad.sp_HistorialCambio_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.HasValue ? (object)fechaInicio.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin.HasValue ? (object)fechaFin.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@TipoDeObservacion", tipoDeObservacion ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new HistorialCambioConsulta()
                            {
                                IdHistorialCambio = Convert.ToInt32(dr["IdHistorialCambio"]),
                                Identificacion = dr["Identificacion"].ToString(),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                NombreEmpleado = dr["NombreEmpleado"].ToString(),
                                NombreTabla = dr["NombreTabla"].ToString(),
                                IdTipoDeObservacion = Convert.ToInt32(dr["IdTipoDeObservacion"]),
                                TipoDeObservacion = dr["TipoDeObservacion"].ToString(),
                                FechaHoraCambio = Convert.ToDateTime(dr["FechaHoraCambio"]),
                                Detalle = dr["Detalle"].ToString(),
                                ValorAnterior = dr["ValorAnterior"].ToString(),
                                ValorNuevo = dr["ValorNuevo"].ToString(),
                                IdRegistro = dr["IdRegistro"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<HistorialCambioConsulta>();
            }

            return lista;
        }
    }
}