
using CapaEntidad.Personas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.personas
{
    public class CD_Cliente
    {
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Cliente_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente()
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

                                CodigoCliente = dr["CodigoCliente"].ToString(),
                                LimiteCredito = dr["LimiteCredito"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["LimiteCredito"]),
                                DiasCredito = dr["DiasCredito"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["DiasCredito"]),

                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaModificacion = dr["FechaModificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaModificacion"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Cliente>();
            }

            return lista;
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Cliente_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", obj.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", string.IsNullOrWhiteSpace(obj.SegundoApellido) ? (object)DBNull.Value : obj.SegundoApellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito.HasValue ? (object)obj.IdDistrito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Direccion", string.IsNullOrWhiteSpace(obj.Direccion) ? (object)DBNull.Value : obj.Direccion);
                    cmd.Parameters.AddWithValue("@LimiteCredito", obj.LimiteCredito.HasValue ? (object)obj.LimiteCredito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiasCredito", obj.DiasCredito.HasValue ? (object)obj.DiasCredito.Value : DBNull.Value);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Cliente_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", obj.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", string.IsNullOrWhiteSpace(obj.SegundoApellido) ? (object)DBNull.Value : obj.SegundoApellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito.HasValue ? (object)obj.IdDistrito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Direccion", string.IsNullOrWhiteSpace(obj.Direccion) ? (object)DBNull.Value : obj.Direccion);
                    cmd.Parameters.AddWithValue("@LimiteCredito", obj.LimiteCredito.HasValue ? (object)obj.LimiteCredito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiasCredito", obj.DiasCredito.HasValue ? (object)obj.DiasCredito.Value : DBNull.Value);
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
                    SqlCommand cmd = new SqlCommand("Persona.sp_Cliente_Inactivar", oconexion);
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
    }
}