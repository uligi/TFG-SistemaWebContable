using CapaEntidad.personas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.personas
{
    public class CD_Empleado
    {
        public List<Empleado> Listar()
        {
            List<Empleado> lista = new List<Empleado>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Empleado_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Empleado()
                            {
                                Identificacion = dr["Identificacion"].ToString(),

                                Nombre = dr["Nombre"].ToString(),
                                PrimerApellido = dr["PrimerApellido"].ToString(),
                                SegundoApellido = dr["SegundoApellido"] == DBNull.Value ? "" : dr["SegundoApellido"].ToString(),
                                FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaNacimiento"]),

                                IdDistrito = dr["IdDistrito"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdDistrito"]),
                                DistritoNombre = dr["DistritoNombre"] == DBNull.Value ? "" : dr["DistritoNombre"].ToString(),

                                IdCanton = dr["IdCanton"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdCanton"]),
                                CantonNombre = dr["CantonNombre"] == DBNull.Value ? "" : dr["CantonNombre"].ToString(),

                                IdProvincia = dr["IdProvincia"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdProvincia"]),
                                ProvinciaNombre = dr["ProvinciaNombre"] == DBNull.Value ? "" : dr["ProvinciaNombre"].ToString(),

                                Direccion = dr["Direccion"] == DBNull.Value ? "" : dr["Direccion"].ToString(),

                                CodigoEmpleado = dr["CodigoEmpleado"].ToString(),
                                IdPuesto = dr["IdPuesto"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdPuesto"]),
                                PuestoNombre = dr["PuestoNombre"] == DBNull.Value ? "" : dr["PuestoNombre"].ToString(),
                                FechaIngreso = dr["FechaIngreso"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaIngreso"]),

                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                RolNombre = dr["RolNombre"].ToString(),

                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                UltimoAcceso = dr["UltimoAcceso"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["UltimoAcceso"]),

                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaModificacion = dr["FechaModificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaModificacion"]),

                                RestablecerClave = Convert.ToBoolean(dr["RestablecerClave"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Empleado>();
            }

            return lista;
        }

        public int Registrar(Empleado obj, out string Mensaje, out string nombreUsuarioGenerado, out string codigoEmpleadoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            nombreUsuarioGenerado = string.Empty;
            codigoEmpleadoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Empleado_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", obj.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", string.IsNullOrWhiteSpace(obj.SegundoApellido) ? (object)DBNull.Value : obj.SegundoApellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito.HasValue ? (object)obj.IdDistrito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Direccion", string.IsNullOrWhiteSpace(obj.Direccion) ? (object)DBNull.Value : obj.Direccion);

                    cmd.Parameters.AddWithValue("@IdPuesto", obj.IdPuesto.HasValue ? (object)obj.IdPuesto.Value : DBNull.Value); cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso.HasValue ? (object)obj.FechaIngreso.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdRol", obj.IdRol);
                    cmd.Parameters.AddWithValue("@ClaveHash", obj.ClaveHash);

                    cmd.Parameters.Add("@NombreUsuarioGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@CodigoEmpleadoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    nombreUsuarioGenerado = cmd.Parameters["@NombreUsuarioGenerado"].Value.ToString();
                    codigoEmpleadoGenerado = cmd.Parameters["@CodigoEmpleadoGenerado"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                Mensaje = ex.Message;
                nombreUsuarioGenerado = string.Empty;
                codigoEmpleadoGenerado = string.Empty;
            }

            return resultado;
        }

        public bool Editar(Empleado obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Empleado_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", obj.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", string.IsNullOrWhiteSpace(obj.SegundoApellido) ? (object)DBNull.Value : obj.SegundoApellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito.HasValue ? (object)obj.IdDistrito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Direccion", string.IsNullOrWhiteSpace(obj.Direccion) ? (object)DBNull.Value : obj.Direccion);

                    cmd.Parameters.AddWithValue("@IdPuesto", obj.IdPuesto.HasValue ? (object)obj.IdPuesto.Value : DBNull.Value); cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso.HasValue ? (object)obj.FechaIngreso.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdRol", obj.IdRol);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);

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

        public bool Inactivar(string identificacion, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Empleado_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion);

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

        public bool RestablecerClave(string identificacion, string claveHash, out string Mensaje, out string nombreUsuario)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            nombreUsuario = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Empleado_RestablecerClave", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion);
                    cmd.Parameters.AddWithValue("@ClaveHash", claveHash);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    nombreUsuario = cmd.Parameters["@NombreUsuario"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
                nombreUsuario = string.Empty;
            }

            return resultado;
        }
    }
}