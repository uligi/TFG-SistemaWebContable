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
                            lista.Add(new Ingreso()
                            {
                                IdIngreso = Convert.ToInt32(dr["IdIngreso"]),

                                IdFactura = dr["IdFactura"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdFactura"]),
                                NumeroFactura = dr["NumeroFactura"] == DBNull.Value ? "" : dr["NumeroFactura"].ToString(),

                                IdAbonoCuentaPorCobrar = dr["IdAbonoCuentaPorCobrar"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdAbonoCuentaPorCobrar"]),

                                IdCuentaContable = dr["IdCuentaContable"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdCuentaContable"]),
                                CodigoCuenta = dr["CodigoCuenta"] == DBNull.Value ? "" : dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"] == DBNull.Value ? "" : dr["NombreCuenta"].ToString(),

                                FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]),
                                Descripcion = dr["Descripcion"].ToString(),

                                IdTipoIngreso = Convert.ToInt32(dr["IdTipoIngreso"]),
                                TipoIngresoNombre = dr["TipoIngresoNombre"].ToString(),

                                OrigenIngreso = dr["OrigenIngreso"].ToString(),
                                Monto = Convert.ToDecimal(dr["Monto"]),

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
                lista = new List<Ingreso>();
            }

            return lista;
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
                                IdFactura = Convert.ToInt32(dr["IdFactura"]),
                                NumeroFactura = dr["NumeroFactura"].ToString(),

                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
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
                                IdAbonoCuentaPorCobrar = Convert.ToInt32(dr["IdAbonoCuentaPorCobrar"]),
                                IdCuentaPorCobrar = Convert.ToInt32(dr["IdCuentaPorCobrar"]),

                                FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                                MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),

                                IdFactura = Convert.ToInt32(dr["IdFactura"]),
                                NumeroFactura = dr["NumeroFactura"].ToString(),

                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString()
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

        public int Registrar(Ingreso obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdFactura", obj.IdFactura == null ? (object)DBNull.Value : obj.IdFactura);
                    cmd.Parameters.AddWithValue("@IdAbonoCuentaPorCobrar", obj.IdAbonoCuentaPorCobrar == null ? (object)DBNull.Value : obj.IdAbonoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@IdTipoIngreso", obj.IdTipoIngreso);
                    cmd.Parameters.AddWithValue("@OrigenIngreso", string.IsNullOrWhiteSpace(obj.OrigenIngreso) ? (object)DBNull.Value : obj.OrigenIngreso);
                    cmd.Parameters.AddWithValue("@Monto", obj.Monto);

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

                    cmd.Parameters.AddWithValue("@IdIngreso", obj.IdIngreso);
                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@FechaIngreso", obj.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@IdTipoIngreso", obj.IdTipoIngreso);
                    cmd.Parameters.AddWithValue("@OrigenIngreso", obj.OrigenIngreso);
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

        public bool Inactivar(int idIngreso, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Ingreso_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdIngreso", idIngreso);

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