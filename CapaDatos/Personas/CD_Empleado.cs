using CapaEntidad.Personas;
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
                                SegundoApellido = dr["SegundoApellido"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),

                                CodigoDistrito = Convert.ToInt32(dr["CodigoDistrito"]),
                                DistritoNombre = dr["DistritoNombre"].ToString(),

                                CodigoCanton = Convert.ToInt32(dr["CodigoCanton"]),
                                CantonNombre = dr["CantonNombre"].ToString(),

                                CodigoProvincia = Convert.ToInt32(dr["CodigoProvincia"]),
                                ProvinciaNombre = dr["ProvinciaNombre"].ToString(),

                                Direccion = dr["Direccion"].ToString(),

                                CodigoEmpleado = dr["CodigoEmpleado"].ToString(),

                                IdPuesto = Convert.ToInt32(dr["IdPuesto"]),
                                PuestoNombre = dr["PuestoNombre"].ToString(),

                                FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]),

                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                RolNombre = dr["RolNombre"].ToString(),

                                NombreUsuario = dr["NombreUsuario"].ToString(),

                                UltimoAcceso = dr["UltimoAcceso"] == DBNull.Value
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(dr["UltimoAcceso"]),

                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

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

        public int Registrar(
            Empleado obj,
            out string Mensaje,
            out string NombreUsuarioGenerado,
            out string CodigoEmpleadoGenerado
        )
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NombreUsuarioGenerado = string.Empty;
            CodigoEmpleadoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Empleado_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", obj.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", obj.SegundoApellido ?? "");
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoDistrito", obj.CodigoDistrito);
                    cmd.Parameters.AddWithValue("@Direccion", obj.Direccion ?? "");

                    cmd.Parameters.AddWithValue("@IdPuesto", obj.IdPuesto);
                    cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso.HasValue ? (object)obj.FechaIngreso.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdRol", obj.IdRol);
                    cmd.Parameters.AddWithValue("@ClaveHash", obj.ClaveHash ?? "");

                    cmd.Parameters.Add("@NombreUsuarioGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@CodigoEmpleadoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    NombreUsuarioGenerado = cmd.Parameters["@NombreUsuarioGenerado"].Value.ToString();
                    CodigoEmpleadoGenerado = cmd.Parameters["@CodigoEmpleadoGenerado"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                Mensaje = ex.Message;
                NombreUsuarioGenerado = string.Empty;
                CodigoEmpleadoGenerado = string.Empty;
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
                    cmd.Parameters.AddWithValue("@SegundoApellido", obj.SegundoApellido ?? "");
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoDistrito", obj.CodigoDistrito);
                    cmd.Parameters.AddWithValue("@Direccion", obj.Direccion ?? "");

                    cmd.Parameters.AddWithValue("@IdPuesto", obj.IdPuesto);
                    cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso.HasValue ? (object)obj.FechaIngreso.Value : DBNull.Value);
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

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");

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

        public bool RestablecerClave(
            string identificacion,
            string claveHash,
            out string Mensaje,
            out string NombreUsuario
        )
        {
            bool resultado = false;
            Mensaje = string.Empty;
            NombreUsuario = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Empleado_RestablecerClave", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");
                    cmd.Parameters.AddWithValue("@ClaveHash", claveHash ?? "");

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    NombreUsuario = cmd.Parameters["@NombreUsuario"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
                NombreUsuario = string.Empty;
            }

            return resultado;
        }
    }
}