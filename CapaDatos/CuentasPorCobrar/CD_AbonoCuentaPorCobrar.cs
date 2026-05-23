using CapaEntidad.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.CuentasPorCobrar
{
    public class CD_AbonoCuentaPorCobrar
    {
        public List<AbonoCuentaPorCobrar> ListarPorCuenta(int idCuentaPorCobrar)
        {
            List<AbonoCuentaPorCobrar> lista = new List<AbonoCuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_ListarPorCuenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPorCobrar", idCuentaPorCobrar);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new AbonoCuentaPorCobrar()
                            {
                                IdAbonoCuentaPorCobrar = Convert.ToInt32(dr["IdAbonoCuentaPorCobrar"]),
                                IdCuentaPorCobrar = Convert.ToInt32(dr["IdCuentaPorCobrar"]),

                                FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                                MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),

                                Observacion = dr["Observacion"] == DBNull.Value ? "" : dr["Observacion"].ToString(),

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
                lista = new List<AbonoCuentaPorCobrar>();
            }

            return lista;
        }

        public int Registrar(AbonoCuentaPorCobrar obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPorCobrar", obj.IdCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@FechaAbono", obj.FechaAbono);
                    cmd.Parameters.AddWithValue("@MontoAbono", obj.MontoAbono);
                    cmd.Parameters.AddWithValue("@Observacion", string.IsNullOrWhiteSpace(obj.Observacion) ? (object)DBNull.Value : obj.Observacion);

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

        public bool Editar(AbonoCuentaPorCobrar obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdAbonoCuentaPorCobrar", obj.IdAbonoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@FechaAbono", obj.FechaAbono);
                    cmd.Parameters.AddWithValue("@MontoAbono", obj.MontoAbono);
                    cmd.Parameters.AddWithValue("@Observacion", string.IsNullOrWhiteSpace(obj.Observacion) ? (object)DBNull.Value : obj.Observacion);
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

        public bool Inactivar(int idAbonoCuentaPorCobrar, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdAbonoCuentaPorCobrar", idAbonoCuentaPorCobrar);

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