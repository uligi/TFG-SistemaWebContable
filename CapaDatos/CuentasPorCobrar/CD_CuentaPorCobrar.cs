using CapaEntidad.Cuentas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Cuentas
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
                            lista.Add(MapearCuentaPorCobrar(dr));
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
                            lista.Add(MapearCuentaPorCobrar(dr));
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

        public List<CuentaPorCobrar> ListarPorCliente(string identificacionCliente)
        {
            List<CuentaPorCobrar> lista = new List<CuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_ListarPorCliente", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCuentaPorCobrar(dr));
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

        public CuentaPorCobrar Obtener(string identificacionCliente, string numeroFactura)
        {
            CuentaPorCobrar obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearCuentaPorCobrar(dr);
                        }
                    }
                }
            }
            catch
            {
                obj = null;
            }

            return obj;
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

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@FechaEmision", obj.FechaEmision);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("@MontoOriginal", obj.MontoOriginal);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado ?? "");

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

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@FechaEmision", obj.FechaEmision);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("@MontoOriginal", obj.MontoOriginal);
                    cmd.Parameters.AddWithValue("@SaldoActual", obj.SaldoActual);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado ?? "");
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

        public bool RecalcularSaldo(string identificacionCliente, string numeroFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_RecalcularSaldo", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

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

        public bool Inactivar(string identificacionCliente, string numeroFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorCobrar_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

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

        private CuentaPorCobrar MapearCuentaPorCobrar(SqlDataReader dr)
        {
            CuentaPorCobrar obj = new CuentaPorCobrar()
            {
                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                NumeroFactura = dr["NumeroFactura"].ToString(),

                FechaFactura = Convert.ToDateTime(dr["FechaFactura"]),
                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),

                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),

                Estado = dr["Estado"].ToString(),

                DiasRestantes = Convert.ToInt32(dr["DiasRestantes"]),
                EstadoCredito = dr["EstadoCredito"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"]),

                ClienteNombre = dr["ClienteNombre"].ToString(),

                TotalFactura = Convert.ToDecimal(dr["TotalFactura"]),
                EstadoFactura = dr["EstadoFactura"].ToString(),
                FacturaActiva = Convert.ToBoolean(dr["FacturaActiva"])
            };

            return obj;
        }
    }
}