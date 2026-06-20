using CapaEntidad.Ubicacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Ubicacion
{
    public class CD_Provincia
    {
        public List<Provincia> Listar()
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Provincia_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Provincia()
                            {
                                CodigoProvincia = Convert.ToInt32(dr["CodigoProvincia"]),
                                Nombre = dr["Nombre"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Provincia>();
            }

            return lista;
        }

        public List<Provincia> ListarActivas()
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Provincia_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            bool activo = Convert.ToBoolean(dr["Activo"]);

                            if (activo)
                            {
                                lista.Add(new Provincia()
                                {
                                    CodigoProvincia = Convert.ToInt32(dr["CodigoProvincia"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Activo = activo
                                });
                            }
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Provincia>();
            }

            return lista;
        }

        public int Registrar(Provincia obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Provincia_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoProvincia", obj.CodigoProvincia);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);

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

        public bool Editar(Provincia obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Provincia_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoProvincia", obj.CodigoProvincia);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
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

        public bool Inactivar(int codigoProvincia, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Provincia_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoProvincia", codigoProvincia);

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