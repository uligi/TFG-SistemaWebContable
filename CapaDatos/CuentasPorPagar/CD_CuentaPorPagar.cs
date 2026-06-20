using CapaEntidad.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.CuentasPorPagar
{
    public class CD_CuentaPorPagar
    {
        public List<CuentaPorPagar> Listar()
        {
            List<CuentaPorPagar> lista = new List<CuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCuentaPorPagar(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaPorPagar>();
            }

            return lista;
        }

        public List<CuentaPorPagar> ListarPendientes()
        {
            List<CuentaPorPagar> lista = new List<CuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_ListarPendientes", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCuentaPorPagar(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaPorPagar>();
            }

            return lista;
        }

        public List<CuentaPorPagar> ListarPorProveedor(string identificacionProveedor)
        {
            List<CuentaPorPagar> lista = new List<CuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_ListarPorProveedor", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCuentaPorPagar(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaPorPagar>();
            }

            return lista;
        }

        public CuentaPorPagar Obtener(string identificacionProveedor, string numeroDocumento)
        {
            CuentaPorPagar obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearCuentaPorPagar(dr);
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

        public int Registrar(CuentaPorPagar obj, out string Mensaje, out string NumeroDocumentoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroDocumentoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", obj.NumeroDocumento ?? "");
                    cmd.Parameters.AddWithValue("@FechaEmision", obj.FechaEmision);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("@Concepto", obj.Concepto ?? "");
                    cmd.Parameters.AddWithValue("@MontoOriginal", obj.MontoOriginal);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado ?? "");

                    cmd.Parameters.Add("@NumeroDocumentoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroDocumentoGenerado = cmd.Parameters["@NumeroDocumentoGenerado"].Value.ToString();
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroDocumentoGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(CuentaPorPagar obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", obj.NumeroDocumento ?? "");
                    cmd.Parameters.AddWithValue("@FechaEmision", obj.FechaEmision);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("@Concepto", obj.Concepto ?? "");
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

        public bool RecalcularSaldo(string identificacionProveedor, string numeroDocumento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_RecalcularSaldo", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento ?? "");

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

        public bool Inactivar(string identificacionProveedor, string numeroDocumento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Cuentas.sp_CuentaPorPagar_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento ?? "");

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

        private CuentaPorPagar MapearCuentaPorPagar(SqlDataReader dr)
        {
            CuentaPorPagar obj = new CuentaPorPagar()
            {
                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                ProveedorNombre = dr["ProveedorNombre"].ToString(),

                DiasCredito = Convert.ToInt32(dr["DiasCredito"]),

                NumeroDocumento = dr["NumeroDocumento"].ToString(),

                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),

                Concepto = dr["Concepto"].ToString(),

                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),

                Estado = dr["Estado"].ToString(),

                DiasRestantes = Convert.ToInt32(dr["DiasRestantes"]),
                EstadoCredito = dr["EstadoCredito"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };

            return obj;
        }
    }
}