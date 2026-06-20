using CapaEntidad.Consultas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Consultas
{
    public class CD_Consultas
    {
        public List<ConsultaFactura> ConsultarFacturas(string filtro, DateTime? fechaInicio, DateTime? fechaFin, string estado)
        {
            List<ConsultaFactura> lista = new List<ConsultaFactura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_Facturas", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.HasValue ? (object)fechaInicio.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin.HasValue ? (object)fechaFin.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", estado ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaFactura()
                            {
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                FechaFactura = Convert.ToDateTime(dr["FechaFactura"]),

                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),

                                IdentificacionEmpleado = dr["IdentificacionEmpleado"].ToString(),
                                EmpleadoNombre = dr["EmpleadoNombre"].ToString(),

                                IdTipoFactura = Convert.ToInt32(dr["IdTipoFactura"]),
                                TipoFacturaNombre = dr["TipoFacturaNombre"].ToString(),

                                IdTipoPago = Convert.ToInt32(dr["IdTipoPago"]),
                                TipoPagoNombre = dr["TipoPagoNombre"].ToString(),

                                SubtotalFactura = Convert.ToDecimal(dr["SubtotalFactura"]),
                                ImpuestoFactura = Convert.ToDecimal(dr["ImpuestoFactura"]),
                                DescuentoFactura = Convert.ToDecimal(dr["DescuentoFactura"]),
                                TotalFactura = Convert.ToDecimal(dr["TotalFactura"]),

                                Estado = dr["Estado"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ConsultaFactura>();
            }

            return lista;
        }

        public List<ConsultaDetalleFactura> ConsultarDetalleFactura(string numeroFactura)
        {
            List<ConsultaDetalleFactura> lista = new List<ConsultaDetalleFactura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_DetalleFactura", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaDetalleFactura()
                            {
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                DescripcionItem = dr["DescripcionItem"].ToString(),

                                Cantidad = Convert.ToDecimal(dr["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                                PorcentajeImpuesto = Convert.ToDecimal(dr["PorcentajeImpuesto"]),
                                PorcentajeDescuento = Convert.ToDecimal(dr["PorcentajeDescuento"]),
                                SubtotalLinea = Convert.ToDecimal(dr["SubtotalLinea"]),
                                TotalLinea = Convert.ToDecimal(dr["TotalLinea"]),

                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ConsultaDetalleFactura>();
            }

            return lista;
        }

        public List<ConsultaCuentaPorCobrar> ConsultarCuentasPorCobrar(string filtro, string estado, string estadoCredito)
        {
            List<ConsultaCuentaPorCobrar> lista = new List<ConsultaCuentaPorCobrar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_CuentasPorCobrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@Estado", estado ?? "");
                    cmd.Parameters.AddWithValue("@EstadoCredito", estadoCredito ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaCuentaPorCobrar()
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
                lista = new List<ConsultaCuentaPorCobrar>();
            }

            return lista;
        }

        public List<ConsultaCuentaPorPagar> ConsultarCuentasPorPagar(string filtro, string estado, string estadoCredito)
        {
            List<ConsultaCuentaPorPagar> lista = new List<ConsultaCuentaPorPagar>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_CuentasPorPagar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@Estado", estado ?? "");
                    cmd.Parameters.AddWithValue("@EstadoCredito", estadoCredito ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaCuentaPorPagar()
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
                lista = new List<ConsultaCuentaPorPagar>();
            }

            return lista;
        }

        public List<ConsultaProducto> ConsultarProductos(string filtro, bool soloBajoStock, bool soloSinStock, bool soloActivos)
        {
            List<ConsultaProducto> lista = new List<ConsultaProducto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_Productos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@SoloBajoStock", soloBajoStock);
                    cmd.Parameters.AddWithValue("@SoloSinStock", soloSinStock);
                    cmd.Parameters.AddWithValue("@SoloActivos", soloActivos);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaProducto()
                            {
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                TipoProducto = dr["TipoProducto"].ToString(),

                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),

                                StockActual = Convert.ToDecimal(dr["StockActual"]),
                                StockMinimo = Convert.ToDecimal(dr["StockMinimo"]),

                                PorcentajeImpuesto = Convert.ToDecimal(dr["PorcentajeImpuesto"]),
                                ImpuestoNombre = dr["ImpuestoNombre"].ToString(),

                                Activo = Convert.ToBoolean(dr["Activo"]),
                                EstadoInventario = dr["EstadoInventario"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ConsultaProducto>();
            }

            return lista;
        }

        public List<ConsultaCliente> ConsultarClientes(string filtro, bool soloConSaldo, bool soloActivos)
        {
            List<ConsultaCliente> lista = new List<ConsultaCliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_Clientes", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@SoloConSaldo", soloConSaldo);
                    cmd.Parameters.AddWithValue("@SoloActivos", soloActivos);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaCliente()
                            {
                                Identificacion = dr["Identificacion"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),

                                LimiteCredito = Convert.ToDecimal(dr["LimiteCredito"]),
                                DiasCredito = Convert.ToInt32(dr["DiasCredito"]),

                                Activo = Convert.ToBoolean(dr["Activo"]),

                                SaldoPendiente = Convert.ToDecimal(dr["SaldoPendiente"]),
                                CantidadFacturasPendientes = Convert.ToInt32(dr["CantidadFacturasPendientes"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ConsultaCliente>();
            }

            return lista;
        }

        public List<ConsultaProveedor> ConsultarProveedores(string filtro, bool soloConSaldo, bool soloActivos)
        {
            List<ConsultaProveedor> lista = new List<ConsultaProveedor>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_Proveedores", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@SoloConSaldo", soloConSaldo);
                    cmd.Parameters.AddWithValue("@SoloActivos", soloActivos);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaProveedor()
                            {
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                NombreContacto = dr["NombreContacto"].ToString(),

                                DiasCredito = Convert.ToInt32(dr["DiasCredito"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),

                                SaldoPendiente = Convert.ToDecimal(dr["SaldoPendiente"]),
                                CantidadDocumentosPendientes = Convert.ToInt32(dr["CantidadDocumentosPendientes"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ConsultaProveedor>();
            }

            return lista;
        }

        public List<ConsultaMovimientoContable> ConsultarMovimientosContables(string filtro, DateTime? fechaInicio, DateTime? fechaFin, string tipoMovimiento)
        {
            List<ConsultaMovimientoContable> lista = new List<ConsultaMovimientoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Consultas.sp_Consulta_MovimientosContables", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.HasValue ? (object)fechaInicio.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin.HasValue ? (object)fechaFin.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", tipoMovimiento ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ConsultaMovimientoContable()
                            {
                                TipoMovimiento = dr["TipoMovimiento"].ToString(),
                                NumeroMovimiento = dr["NumeroMovimiento"].ToString(),
                                FechaMovimiento = Convert.ToDateTime(dr["FechaMovimiento"]),

                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),

                                Descripcion = dr["Descripcion"].ToString(),
                                Monto = Convert.ToDecimal(dr["Monto"]),

                                Origen = dr["Origen"].ToString(),
                                Referencia = dr["Referencia"].ToString(),

                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ConsultaMovimientoContable>();
            }

            return lista;
        }
    }
}