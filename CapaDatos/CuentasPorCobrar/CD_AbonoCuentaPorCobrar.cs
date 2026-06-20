using CapaEntidad.Cuentas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Cuentas
{
    public class CD_AbonoCuentaPorCobrar
    {
        public List<AbonoCuentaPorCobrar> Listar()
        {
            List<AbonoCuentaPorCobrar> lista = new List<AbonoCuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearAbonoCuentaPorCobrar(dr));
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

        public List<AbonoCuentaPorCobrar> ListarPorCuenta(string identificacionCliente, string numeroFactura)
        {
            List<AbonoCuentaPorCobrar> lista = new List<AbonoCuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_ListarPorCuenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearAbonoCuentaPorCobrar(dr));
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

        public AbonoCuentaPorCobrar Obtener(string identificacionCliente, string numeroFactura, int numeroAbono)
        {
            AbonoCuentaPorCobrar obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAbono", numeroAbono);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearAbonoCuentaPorCobrar(dr);
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

        public int Registrar(AbonoCuentaPorCobrar obj, out string Mensaje, out int NumeroAbonoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroAbonoGenerado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@FechaAbono", obj.FechaAbono);
                    cmd.Parameters.AddWithValue("@MontoAbono", obj.MontoAbono);
                    cmd.Parameters.AddWithValue("@Observacion", obj.Observacion ?? "");

                    cmd.Parameters.Add("@NumeroAbonoGenerado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroAbonoGenerado = Convert.ToInt32(cmd.Parameters["@NumeroAbonoGenerado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroAbonoGenerado = 0;
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

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAbono", obj.NumeroAbono);
                    cmd.Parameters.AddWithValue("@FechaAbono", obj.FechaAbono);
                    cmd.Parameters.AddWithValue("@MontoAbono", obj.MontoAbono);
                    cmd.Parameters.AddWithValue("@Observacion", obj.Observacion ?? "");
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

        public bool Inactivar(string identificacionCliente, string numeroFactura, int numeroAbono, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorCobrar_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAbono", numeroAbono);

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

        private AbonoCuentaPorCobrar MapearAbonoCuentaPorCobrar(SqlDataReader dr)
        {
            return new AbonoCuentaPorCobrar()
            {
                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                NumeroFactura = dr["NumeroFactura"].ToString(),
                NumeroAbono = Convert.ToInt32(dr["NumeroAbono"]),

                FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),
                Observacion = dr["Observacion"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"]),

                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),
                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),
                EstadoCuenta = dr["EstadoCuenta"].ToString(),

                ClienteNombre = dr["ClienteNombre"].ToString()
            };
        }
    }
}