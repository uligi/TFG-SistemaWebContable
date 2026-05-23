using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_DetalleAsientoContable
    {
        public List<DetalleAsientoContable> ListarPorAsiento(int idAsientoContable)
        {
            List<DetalleAsientoContable> lista = new List<DetalleAsientoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_ListarPorAsiento", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdAsientoContable", idAsientoContable);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleAsientoContable()
                            {
                                IdDetalleAsientoContable = Convert.ToInt32(dr["IdDetalleAsientoContable"]),
                                IdAsientoContable = Convert.ToInt32(dr["IdAsientoContable"]),
                                IdCuentaContable = Convert.ToInt32(dr["IdCuentaContable"]),

                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),

                                Debe = Convert.ToDecimal(dr["Debe"]),
                                Haber = Convert.ToDecimal(dr["Haber"]),

                                DescripcionLinea = dr["DescripcionLinea"] == DBNull.Value ? "" : dr["DescripcionLinea"].ToString(),

                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DetalleAsientoContable>();
            }

            return lista;
        }

        public int Registrar(DetalleAsientoContable obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdAsientoContable", obj.IdAsientoContable);
                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@Debe", obj.Debe);
                    cmd.Parameters.AddWithValue("@Haber", obj.Haber);
                    cmd.Parameters.AddWithValue("@DescripcionLinea", string.IsNullOrWhiteSpace(obj.DescripcionLinea) ? (object)DBNull.Value : obj.DescripcionLinea);

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

        public bool Editar(DetalleAsientoContable obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDetalleAsientoContable", obj.IdDetalleAsientoContable);
                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@Debe", obj.Debe);
                    cmd.Parameters.AddWithValue("@Haber", obj.Haber);
                    cmd.Parameters.AddWithValue("@DescripcionLinea", string.IsNullOrWhiteSpace(obj.DescripcionLinea) ? (object)DBNull.Value : obj.DescripcionLinea);
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

        public bool Inactivar(int idDetalleAsientoContable, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDetalleAsientoContable", idDetalleAsientoContable);

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