using CapaEntidad.Personas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.personas
{
    public class CD_Telefono
    {
        public List<Telefono> Listar()
        {
            List<Telefono> lista = new List<Telefono>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Telefono_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Telefono()
                            {
                                Identificacion = dr["Identificacion"].ToString(),
                                NumeroTelefono = dr["NumeroTelefono"].ToString(),
                                NumeroTelefonoAnterior = dr["NumeroTelefono"].ToString(),

                                IdTipoTelefono = Convert.ToInt32(dr["IdTipoTelefono"]),
                                TipoTelefonoNombre = dr["TipoTelefonoNombre"].ToString(),

                                EsPrincipal = Convert.ToBoolean(dr["EsPrincipal"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),

                                Nombre = dr["Nombre"].ToString(),
                                PrimerApellido = dr["PrimerApellido"].ToString(),
                                SegundoApellido = dr["SegundoApellido"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Telefono>();
            }

            return lista;
        }

        public List<Telefono> ListarPorPersona(string identificacion)
        {
            List<Telefono> lista = new List<Telefono>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Telefono_ListarPorPersona", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Telefono()
                            {
                                Identificacion = dr["Identificacion"].ToString(),
                                NumeroTelefono = dr["NumeroTelefono"].ToString(),
                                NumeroTelefonoAnterior = dr["NumeroTelefono"].ToString(),

                                IdTipoTelefono = Convert.ToInt32(dr["IdTipoTelefono"]),
                                TipoTelefonoNombre = dr["TipoTelefonoNombre"].ToString(),

                                EsPrincipal = Convert.ToBoolean(dr["EsPrincipal"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),

                                Nombre = dr["Nombre"].ToString(),
                                PrimerApellido = dr["PrimerApellido"].ToString(),
                                SegundoApellido = dr["SegundoApellido"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Telefono>();
            }

            return lista;
        }

        public int Registrar(Telefono obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Telefono_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@NumeroTelefono", obj.NumeroTelefono);
                    cmd.Parameters.AddWithValue("@IdTipoTelefono", obj.IdTipoTelefono);
                    cmd.Parameters.AddWithValue("@EsPrincipal", obj.EsPrincipal);

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

        public bool Editar(Telefono obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Telefono_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@NumeroTelefonoAnterior", obj.NumeroTelefonoAnterior);
                    cmd.Parameters.AddWithValue("@NumeroTelefonoNuevo", obj.NumeroTelefono);
                    cmd.Parameters.AddWithValue("@IdTipoTelefono", obj.IdTipoTelefono);
                    cmd.Parameters.AddWithValue("@EsPrincipal", obj.EsPrincipal);
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

        public bool Inactivar(string identificacion, string numeroTelefono, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Telefono_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? "");
                    cmd.Parameters.AddWithValue("@NumeroTelefono", numeroTelefono ?? "");

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