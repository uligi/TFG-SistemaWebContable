using CapaEntidad.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.CuentasPorPagar
{
    public class CD_AbonoCuentaPorPagar
    {
        public List<AbonoCuentaPorPagar> ListarPorCuenta(int idCuentaPorPagar)
        {
            List<AbonoCuentaPorPagar> lista = new List<AbonoCuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_ListarPorCuenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPorPagar", idCuentaPorPagar);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new AbonoCuentaPorPagar()
                            {
                                IdAbonoCuentaPorPagar = Convert.ToInt32(dr["IdAbonoCuentaPorPagar"]),
                                IdCuentaPorPagar = Convert.ToInt32(dr["IdCuentaPorPagar"]),

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
                lista = new List<AbonoCuentaPorPagar>();
            }

            return lista;
        }

        public int Registrar(AbonoCuentaPorPagar obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPorPagar", obj.IdCuentaPorPagar);
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

        public bool Editar(AbonoCuentaPorPagar obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdAbonoCuentaPorPagar", obj.IdAbonoCuentaPorPagar);
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

        public bool Inactivar(int idAbonoCuentaPorPagar, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdAbonoCuentaPorPagar", idAbonoCuentaPorPagar);

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