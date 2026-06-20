using CapaEntidad.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.CuentasPorPagar
{
    public class CD_AbonoCuentaPorPagar
    {
        public List<AbonoCuentaPorPagar> Listar()
        {
            List<AbonoCuentaPorPagar> lista = new List<AbonoCuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearAbonoCuentaPorPagar(dr));
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

        public List<AbonoCuentaPorPagar> ListarPorCuenta(string identificacionProveedor, string numeroDocumento)
        {
            List<AbonoCuentaPorPagar> lista = new List<AbonoCuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_ListarPorCuenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearAbonoCuentaPorPagar(dr));
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

        public AbonoCuentaPorPagar Obtener(string identificacionProveedor, string numeroDocumento, int numeroAbono)
        {
            AbonoCuentaPorPagar obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAbono", numeroAbono);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearAbonoCuentaPorPagar(dr);
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

        public int Registrar(AbonoCuentaPorPagar obj, out string Mensaje, out int NumeroAbonoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroAbonoGenerado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", obj.NumeroDocumento ?? "");
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

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", obj.NumeroDocumento ?? "");
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

        public bool Inactivar(string identificacionProveedor, string numeroDocumento, int numeroAbono, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_AbonoCuentaPorPagar_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento ?? "");
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

        private AbonoCuentaPorPagar MapearAbonoCuentaPorPagar(SqlDataReader dr)
        {
            AbonoCuentaPorPagar obj = new AbonoCuentaPorPagar()
            {
                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                ProveedorNombre = dr["ProveedorNombre"].ToString(),

                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                NumeroAbono = Convert.ToInt32(dr["NumeroAbono"]),

                FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),
                Observacion = dr["Observacion"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),
                Activo = Convert.ToBoolean(dr["Activo"]),

                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),
                Concepto = dr["Concepto"].ToString(),

                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),
                EstadoCuenta = dr["EstadoCuenta"].ToString()
            };

            return obj;
        }
    }
}