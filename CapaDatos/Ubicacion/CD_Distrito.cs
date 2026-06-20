using CapaEntidad.Ubicacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Ubicacion
{
    public class CD_Distrito
    {
        public List<Distrito> Listar()
        {
            List<Distrito> lista = new List<Distrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Distrito_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Distrito()
                            {
                                CodigoDistrito = Convert.ToInt32(dr["CodigoDistrito"]),
                                CodigoCanton = Convert.ToInt32(dr["CodigoCanton"]),
                                Nombre = dr["Nombre"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                CantonNombre = dr["CantonNombre"].ToString(),
                                CodigoProvincia = Convert.ToInt32(dr["CodigoProvincia"]),
                                ProvinciaNombre = dr["ProvinciaNombre"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Distrito>();
            }

            return lista;
        }

        public int Registrar(Distrito obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Distrito_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoDistrito", obj.CodigoDistrito);
                    cmd.Parameters.AddWithValue("@CodigoCanton", obj.CodigoCanton);
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

        public bool Editar(Distrito obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Distrito_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoDistrito", obj.CodigoDistrito);
                    cmd.Parameters.AddWithValue("@CodigoCanton", obj.CodigoCanton);
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

        public bool Inactivar(int codigoDistrito, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Distrito_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoDistrito", codigoDistrito);

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

        public List<Distrito> ListarActivosPorCanton(int codigoCanton)
        {
            List<Distrito> lista = new List<Distrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Ubicacion.sp_Distrito_ListarActivosPorCanton", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoCanton", codigoCanton);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Distrito()
                            {
                                CodigoDistrito = Convert.ToInt32(dr["CodigoDistrito"]),
                                CodigoCanton = Convert.ToInt32(dr["CodigoCanton"]),
                                Nombre = dr["Nombre"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Distrito>();
            }

            return lista;
        }
    }
}