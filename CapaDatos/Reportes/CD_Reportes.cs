using CapaEntidad.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Reportes
{
    public class CD_Reportes
    {
        public ReporteResumenFinanciero ObtenerResumenFinanciero(DateTime fechaInicio, DateTime fechaFin)
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
                            obj.MargenUtilidad = Convert.ToDecimal(dr["MargenUtilidad"]);
                            obj.CapitalNetoPendiente = Convert.ToDecimal(dr["CapitalNetoPendiente"]);
                            obj.EstadoFinanciero = dr["EstadoFinanciero"].ToString();
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
                                NumeroIngreso = dr["NumeroIngreso"].ToString(),
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
                                NumeroGasto = dr["NumeroGasto"].ToString(),
                                FechaGasto = Convert.ToDateTime(dr["FechaGasto"]),
                                TipoGasto = dr["TipoGasto"].ToString(),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                ProveedorNombre = dr["ProveedorNombre"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Monto = Convert.ToDecimal(dr["Monto"]),
                                NumeroComprobante = dr["NumeroComprobante"].ToString(),
                                NombreArchivoComprobante = dr["NombreArchivoComprobante"].ToString(),
                                RutaComprobante = dr["RutaComprobante"].ToString(),
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
                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),
                                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),
                                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),
                                Estado = dr["Estado"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                DiasRestantes = Convert.ToInt32(dr["DiasRestantes"]),
                                EstadoCredito = dr["EstadoCredito"].ToString()
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
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                ProveedorNombre = dr["ProveedorNombre"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                FechaEmision = Convert.ToDateTime(dr["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),
                                Concepto = dr["Concepto"].ToString(),
                                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),
                                Estado = dr["Estado"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                DiasRestantes = Convert.ToInt32(dr["DiasRestantes"]),
                                EstadoCredito = dr["EstadoCredito"].ToString()
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

        public List<ReporteProductoMasVendido> ReporteProductosMasVendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteProductoMasVendido> lista = new List<ReporteProductoMasVendido>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_ProductosMasVendidos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteProductoMasVendido()
                            {
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                TipoProducto = dr["TipoProducto"].ToString(),
                                CantidadVendida = Convert.ToDecimal(dr["CantidadVendida"]),
                                TotalVendido = Convert.ToDecimal(dr["TotalVendido"]),
                                VecesFacturado = Convert.ToInt32(dr["VecesFacturado"]),
                                StockActual = Convert.ToDecimal(dr["StockActual"]),
                                Recomendacion = dr["Recomendacion"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ReporteProductoMasVendido>();
            }

            return lista;
        }

        public List<ReporteClienteMasVenta> ReporteClientesMasVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteClienteMasVenta> lista = new List<ReporteClienteMasVenta>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_ClientesMasVentas", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteClienteMasVenta()
                            {
                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),
                                CantidadFacturas = Convert.ToInt32(dr["CantidadFacturas"]),
                                TotalComprado = Convert.ToDecimal(dr["TotalComprado"]),
                                PromedioCompra = Convert.ToDecimal(dr["PromedioCompra"]),
                                UltimaCompra = Convert.ToDateTime(dr["UltimaCompra"]),
                                LimiteCredito = Convert.ToDecimal(dr["LimiteCredito"]),
                                DiasCredito = Convert.ToInt32(dr["DiasCredito"]),
                                ClasificacionCliente = dr["ClasificacionCliente"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ReporteClienteMasVenta>();
            }

            return lista;
        }

        public List<ReporteFlujoDiario> ReporteFlujoDiario(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteFlujoDiario> lista = new List<ReporteFlujoDiario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Reportes.sp_Reporte_FlujoDiario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteFlujoDiario()
                            {
                                Fecha = Convert.ToDateTime(dr["Fecha"]),
                                TotalIngresos = Convert.ToDecimal(dr["TotalIngresos"]),
                                TotalGastos = Convert.ToDecimal(dr["TotalGastos"]),
                                ResultadoDiario = Convert.ToDecimal(dr["ResultadoDiario"]),
                                EstadoDia = dr["EstadoDia"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ReporteFlujoDiario>();
            }

            return lista;
        }
    }
}