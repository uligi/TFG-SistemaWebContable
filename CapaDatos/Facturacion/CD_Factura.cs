using CapaEntidad.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Facturacion
{
    public class CD_Factura
    {
        public List<Factura> Listar()
        {
            List<Factura> lista = new List<Factura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearFactura(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Factura>();
            }

            return lista;
        }

        public List<Factura> ListarActivas()
        {
            List<Factura> lista = new List<Factura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_ListarActivas", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearFactura(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Factura>();
            }

            return lista;
        }

        public List<Factura> ListarPorCliente(string identificacionCliente)
        {
            List<Factura> lista = new List<Factura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_ListarPorCliente", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", identificacionCliente ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearFactura(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Factura>();
            }

            return lista;
        }

        public Factura Obtener(string numeroFactura)
        {
            Factura obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearFactura(dr);
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

        public int Registrar(Factura obj, out string Mensaje, out string NumeroFacturaGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroFacturaGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", obj.IdentificacionEmpleado ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoPago", obj.IdTipoPago);
                    cmd.Parameters.AddWithValue("@IdTipoFactura", obj.IdTipoFactura);
                    cmd.Parameters.AddWithValue("@FechaFactura", obj.FechaFactura);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado ?? "");

                    cmd.Parameters.Add("@NumeroFacturaGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroFacturaGenerado = cmd.Parameters["@NumeroFacturaGenerado"].Value.ToString();
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroFacturaGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(Factura obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente ?? "");
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", obj.IdentificacionEmpleado ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoPago", obj.IdTipoPago);
                    cmd.Parameters.AddWithValue("@IdTipoFactura", obj.IdTipoFactura);
                    cmd.Parameters.AddWithValue("@FechaFactura", obj.FechaFactura);
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

        public bool CambiarEstado(string numeroFactura, string estado, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_CambiarEstado", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@Estado", estado ?? "");

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

        public bool RecalcularTotales(string numeroFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_RecalcularTotales", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = true;
                    Mensaje = "Totales de factura recalculados correctamente.";
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Inactivar(string numeroFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

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

        private Factura MapearFactura(SqlDataReader dr)
        {
            return new Factura()
            {
                NumeroFactura = dr["NumeroFactura"].ToString(),

                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                ClienteNombre = dr["ClienteNombre"].ToString(),

                IdentificacionEmpleado = dr["IdentificacionEmpleado"].ToString(),
                EmpleadoNombre = dr["EmpleadoNombre"].ToString(),

                IdTipoPago = Convert.ToInt32(dr["IdTipoPago"]),
                TipoPagoNombre = dr["TipoPagoNombre"].ToString(),

                IdTipoFactura = Convert.ToInt32(dr["IdTipoFactura"]),
                TipoFacturaNombre = dr["TipoFacturaNombre"].ToString(),

                FechaFactura = Convert.ToDateTime(dr["FechaFactura"]),

                Subtotal = Convert.ToDecimal(dr["Subtotal"]),
                TotalImpuesto = Convert.ToDecimal(dr["TotalImpuesto"]),
                TotalDescuento = Convert.ToDecimal(dr["TotalDescuento"]),
                TotalFactura = Convert.ToDecimal(dr["TotalFactura"]),

                Estado = dr["Estado"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };
        }

        public bool Emitir(string numeroFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Emitir", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

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
    }
}