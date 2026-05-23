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
                            lista.Add(new Factura()
                            {
                                IdFactura = Convert.ToInt32(dr["IdFactura"]),

                                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                                ClienteNombre = dr["ClienteNombre"].ToString(),

                                IdentificacionEmpleado = dr["IdentificacionEmpleado"].ToString(),
                                EmpleadoNombre = dr["EmpleadoNombre"].ToString(),

                                NumeroFactura = dr["NumeroFactura"].ToString(),

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
                                FechaModificacion = dr["FechaModificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaModificacion"]),

                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
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

        public int Registrar(Factura obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente);
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", obj.IdentificacionEmpleado);
                    cmd.Parameters.AddWithValue("@IdTipoPago", obj.IdTipoPago);
                    cmd.Parameters.AddWithValue("@IdTipoFactura", obj.IdTipoFactura);
                    cmd.Parameters.AddWithValue("@FechaFactura", obj.FechaFactura);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrWhiteSpace(obj.Estado) ? "Borrador" : obj.Estado);

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

                    cmd.Parameters.AddWithValue("@IdFactura", obj.IdFactura);
                    cmd.Parameters.AddWithValue("@IdentificacionCliente", obj.IdentificacionCliente);
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", obj.IdentificacionEmpleado);
                    cmd.Parameters.AddWithValue("@IdTipoPago", obj.IdTipoPago);
                    cmd.Parameters.AddWithValue("@IdTipoFactura", obj.IdTipoFactura);
                    cmd.Parameters.AddWithValue("@FechaFactura", obj.FechaFactura);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado);
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

        public bool Inactivar(int idFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdFactura", idFactura);

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

        public bool Emitir(int idFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_Factura_Emitir", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdFactura", idFactura);

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