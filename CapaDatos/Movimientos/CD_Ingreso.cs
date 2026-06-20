using CapaEntidad.Movimientos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Movimientos
{
    public class CD_Ingreso
    {
        public List<Ingreso> Listar()
        {
            List<Ingreso> lista = new List<Ingreso>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearIngreso(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Ingreso>();
            }

            return lista;
        }

        public List<Ingreso> ListarActivos()
        {
            List<Ingreso> lista = new List<Ingreso>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_ListarActivos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearIngreso(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Ingreso>();
            }

            return lista;
        }

        public Ingreso Obtener(string numeroIngreso)
        {
            Ingreso obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroIngreso", numeroIngreso ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearIngreso(dr);
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

        public int Registrar(Ingreso obj, out string Mensaje, out string NumeroIngresoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroIngresoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoIngreso", obj.IdTipoIngreso);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");

                    cmd.Parameters.AddWithValue("@IdentificacionClienteAbono", obj.IdentificacionClienteAbono ?? "");
                    cmd.Parameters.AddWithValue("@NumeroFacturaAbono", obj.NumeroFacturaAbono ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAbonoCuentaPorCobrar", obj.NumeroAbonoCuentaPorCobrar);

                    cmd.Parameters.AddWithValue("@OrigenIngreso", obj.OrigenIngreso ?? "");
                    cmd.Parameters.AddWithValue("@Monto", obj.Monto);

                    cmd.Parameters.Add("@NumeroIngresoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroIngresoGenerado = cmd.Parameters["@NumeroIngresoGenerado"].Value.ToString();
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroIngresoGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(Ingreso obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroIngreso", obj.NumeroIngreso ?? "");
                    cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoIngreso", obj.IdTipoIngreso);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@OrigenIngreso", obj.OrigenIngreso ?? "");
                    cmd.Parameters.AddWithValue("@Monto", obj.Monto);
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

        public bool Inactivar(string numeroIngreso, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroIngreso", numeroIngreso ?? "");

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

        public List<FacturaContadoPendiente> ListarFacturasContadoPendientes()
        {
            List<FacturaContadoPendiente> lista = new List<FacturaContadoPendiente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_ListarFacturasContadoPendientes", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new FacturaContadoPendiente()
                            {
                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),
                                FechaFactura = Convert.ToDateTime(dr["FechaFactura"]),
                                TotalFactura = Convert.ToDecimal(dr["TotalFactura"]),
                                TipoFactura = dr["TipoFactura"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<FacturaContadoPendiente>();
            }

            return lista;
        }

        public List<AbonoPendienteIngreso> ListarAbonosPendientes()
        {
            List<AbonoPendienteIngreso> lista = new List<AbonoPendienteIngreso>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_ListarAbonosPendientes", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new AbonoPendienteIngreso()
                            {
                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                NumeroAbono = Convert.ToInt32(dr["NumeroAbono"]),
                                FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                                MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),
                                Observacion = dr["Observacion"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),
                                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),
                                EstadoCuenta = dr["EstadoCuenta"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<AbonoPendienteIngreso>();
            }

            return lista;
        }

        private Ingreso MapearIngreso(SqlDataReader dr)
        {
            Ingreso obj = new Ingreso()
            {
                NumeroIngreso = dr["NumeroIngreso"].ToString(),
                FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]),
                Descripcion = dr["Descripcion"].ToString(),

                IdTipoIngreso = Convert.ToInt32(dr["IdTipoIngreso"]),
                TipoIngresoNombre = dr["TipoIngresoNombre"].ToString(),

                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                NombreCuenta = dr["NombreCuenta"].ToString(),

                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                NumeroFactura = dr["NumeroFactura"].ToString(),
                ClienteNombre = dr["ClienteNombre"].ToString(),

                IdentificacionClienteAbono = dr["IdentificacionClienteAbono"].ToString(),
                NumeroFacturaAbono = dr["NumeroFacturaAbono"].ToString(),
                NumeroAbonoCuentaPorCobrar = Convert.ToInt32(dr["NumeroAbonoCuentaPorCobrar"]),

                OrigenIngreso = dr["OrigenIngreso"].ToString(),
                Monto = Convert.ToDecimal(dr["Monto"]),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };

            if (dr["FechaAbono"] != DBNull.Value)
            {
                obj.FechaAbono = Convert.ToDateTime(dr["FechaAbono"]);
            }

            if (dr["MontoAbono"] != DBNull.Value)
            {
                obj.MontoAbono = Convert.ToDecimal(dr["MontoAbono"]);
            }

            return obj;
        }
    }
}