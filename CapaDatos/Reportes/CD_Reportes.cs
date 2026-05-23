using CapaEntidad.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Reportes
{
    public class CD_Reportes
    {
        public ReporteResumenFinanciero ResumenFinanciero(DateTime fechaInicio, DateTime fechaFin)
        {
            ReporteResumenFinanciero obj = new ReporteResumenFinanciero();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_ResumenFinanciero", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj.TotalIngresos = Convert.ToDecimal(dr["TotalIngresos"]);
                            obj.TotalGastos = Convert.ToDecimal(dr["TotalGastos"]);
                            obj.UtilidadEstimada = Convert.ToDecimal(dr["UtilidadEstimada"]);
                            obj.TotalCuentasPorCobrar = Convert.ToDecimal(dr["TotalCuentasPorCobrar"]);
                            obj.TotalCuentasPorPagar = Convert.ToDecimal(dr["TotalCuentasPorPagar"]);
                        }
                    }
                }
            }
            catch
            {
                obj = new ReporteResumenFinanciero();
            }

            return obj;
        }

        public List<ReporteIngreso> ReporteIngresos(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteIngreso> lista = new List<ReporteIngreso>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_Ingresos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteIngreso()
                            {
                                IdIngreso = Convert.ToInt32(dr["IdIngreso"]),
                                FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]),
                                TipoIngreso = dr["TipoIngreso"].ToString(),
                                OrigenIngreso = dr["OrigenIngreso"].ToString(),
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Monto = Convert.ToDecimal(dr["Monto"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ReporteIngreso>();
            }

            return lista;
        }

        public List<ReporteGasto> ReporteGastos(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteGasto> lista = new List<ReporteGasto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_Gastos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteGasto()
                            {
                                IdGasto = Convert.ToInt32(dr["IdGasto"]),
                                FechaGasto = Convert.ToDateTime(dr["FechaGasto"]),
                                TipoGasto = dr["TipoGasto"].ToString(),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                ProveedorNombre = dr["ProveedorNombre"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Monto = Convert.ToDecimal(dr["Monto"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ReporteGasto>();
            }

            return lista;
        }

        public List<ReporteCuentaPorCobrar> ReporteCuentasPorCobrar()
        {
            List<ReporteCuentaPorCobrar> lista = new List<ReporteCuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_CuentasPorCobrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteCuentaPorCobrar()
                            {
                                IdCuentaPorCobrar = Convert.ToInt32(dr["IdCuentaPorCobrar"]),
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),
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
                lista = new List<ReporteCuentaPorCobrar>();
            }

            return lista;
        }

        public List<ReporteCuentaPorPagar> ReporteCuentasPorPagar()
        {
            List<ReporteCuentaPorPagar> lista = new List<ReporteCuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_CuentasPorPagar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteCuentaPorPagar()
                            {
                                IdCuentaPorPagar = Convert.ToInt32(dr["IdCuentaPorPagar"]),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                ProveedorNombre = dr["ProveedorNombre"].ToString(),
                                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),
                                Concepto = dr["Concepto"].ToString(),
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
                lista = new List<ReporteCuentaPorPagar>();
            }

            return lista;
        }
    }
}