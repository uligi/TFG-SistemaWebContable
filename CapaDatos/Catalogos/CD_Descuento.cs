using CapaEntidad.Catalogos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Catalogos
{
    public class CD_Descuento
    {
        public List<Descuento> Listar()
        {
            List<Descuento> lista = new List<Descuento>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Catalogos.sp_Descuento_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Descuento()
                            {
                                IdDescuento = Convert.ToInt32(dr["IdDescuento"]),
                                Nombre = dr["Nombre"].ToString(),
                                Porcentaje = Convert.ToDecimal(dr["Porcentaje"]),
                                RequiereAutorizacion = Convert.ToBoolean(dr["RequiereAutorizacion"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Descuento>();
            }

            return lista;
        }

        public List<Descuento> ListarActivos()
        {
            List<Descuento> lista = new List<Descuento>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Catalogos.sp_Descuento_ListarActivos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Descuento()
                            {
                                IdDescuento = Convert.ToInt32(dr["IdDescuento"]),
                                Nombre = dr["Nombre"].ToString(),
                                Porcentaje = Convert.ToDecimal(dr["Porcentaje"]),
                                RequiereAutorizacion = Convert.ToBoolean(dr["RequiereAutorizacion"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Descuento>();
            }

            return lista;
        }

        public int Registrar(Descuento obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Catalogos.sp_Descuento_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Porcentaje", obj.Porcentaje);
                    cmd.Parameters.AddWithValue("@RequiereAutorizacion", obj.RequiereAutorizacion);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");

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

        public bool Editar(Descuento obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Catalogos.sp_Descuento_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDescuento", obj.IdDescuento);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Porcentaje", obj.Porcentaje);
                    cmd.Parameters.AddWithValue("@RequiereAutorizacion", obj.RequiereAutorizacion);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
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

        public bool Inactivar(int idDescuento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Catalogos.sp_Descuento_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDescuento", idDescuento);

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