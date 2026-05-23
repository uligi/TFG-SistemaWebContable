using CapaEntidad.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.CuentasPorCobrar
{
    public class CD_CuentaPorCobrar
    {
        public List<CuentaPorCobrar> Listar()
        {
            List<CuentaPorCobrar> lista = new List<CuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CuentaPorCobrar()
                            {
                                IdCuentaPorCobrar = Convert.ToInt32(dr["IdCuentaPorCobrar"]),

                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),

                                IdFactura = Convert.ToInt32(dr["IdFactura"]),
                                NumeroFactura = dr["NumeroFactura"].ToString(),

                                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),

                                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),

                                Estado = dr["Estado"].ToString(),

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
                lista = new List<CuentaPorCobrar>();
            }

            return lista;
        }

        public List<CuentaPorCobrar> ListarPendientes()
        {
            List<CuentaPorCobrar> lista = new List<CuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_ListarPendientes", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CuentaPorCobrar()
                            {
                                IdCuentaPorCobrar = Convert.ToInt32(dr["IdCuentaPorCobrar"]),

                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),

                                IdFactura = Convert.ToInt32(dr["IdFactura"]),
                                NumeroFactura = dr["NumeroFactura"].ToString(),

                                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),

                                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),

                                Estado = dr["Estado"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaPorCobrar>();
            }

            return lista;
        }

        public int Registrar(CuentaPorCobrar obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente);
                    cmd.Parameters.AddWithValue("@IdFactura", obj.IdFactura);
                    cmd.Parameters.AddWithValue("@FechaEmision", obj.FechaEmision);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("@MontoOriginal", obj.MontoOriginal);

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

        public bool Editar(CuentaPorCobrar obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPorCobrar", obj.IdCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("@MontoOriginal", obj.MontoOriginal);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado);
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

        public bool Inactivar(int idCuentaPorCobrar, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPorCobrar", idCuentaPorCobrar);

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