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
                                SegundoApellido = dr["SegundoApellido"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),

                                CodigoDistrito = Convert.ToInt32(dr["CodigoDistrito"]),
                                DistritoNombre = dr["DistritoNombre"].ToString(),

                                CodigoCanton = Convert.ToInt32(dr["CodigoCanton"]),
                                CantonNombre = dr["CantonNombre"].ToString(),

                                CodigoProvincia = Convert.ToInt32(dr["CodigoProvincia"]),
                                ProvinciaNombre = dr["ProvinciaNombre"].ToString(),

                                Direccion = dr["Direccion"].ToString(),

                                CodigoCliente = dr["CodigoCliente"].ToString(),
                                LimiteCredito = Convert.ToDecimal(dr["LimiteCredito"]),
                                DiasCredito = Convert.ToInt32(dr["DiasCredito"]),

                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),
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
                    cmd.Parameters.AddWithValue("@SegundoApellido", obj.SegundoApellido ?? "");
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoDistrito", obj.CodigoDistrito);
                    cmd.Parameters.AddWithValue("@Direccion", obj.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@LimiteCredito", obj.LimiteCredito);
                    cmd.Parameters.AddWithValue("@DiasCredito", obj.DiasCredito);

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
                    cmd.Parameters.AddWithValue("@SegundoApellido", obj.SegundoApellido ?? "");
                    cmd.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento.HasValue ? (object)obj.FechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoDistrito", obj.CodigoDistrito);
                    cmd.Parameters.AddWithValue("@Direccion", obj.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@LimiteCredito", obj.LimiteCredito);
                    cmd.Parameters.AddWithValue("@DiasCredito", obj.DiasCredito);
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