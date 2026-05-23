using CapaEntidad.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Facturacion
{
    public class CD_DetalleFactura
    {
        public List<DetalleFactura> ListarPorFactura(int idFactura)
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_ListarPorFactura", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdFactura", idFactura);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleFactura()
                            {
                                IdDetalleFactura = Convert.ToInt32(dr["IdDetalleFactura"]),
                                IdFactura = Convert.ToInt32(dr["IdFactura"]),

                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                ProductoNombre = dr["ProductoNombre"].ToString(),

                                IdImpuesto = Convert.ToInt32(dr["IdImpuesto"]),
                                ImpuestoNombre = dr["ImpuestoNombre"].ToString(),
                                PorcentajeImpuesto = Convert.ToDecimal(dr["PorcentajeImpuesto"]),

                                IdDescuento = dr["IdDescuento"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdDescuento"]),
                                DescuentoNombre = dr["DescuentoNombre"] == DBNull.Value ? "" : dr["DescuentoNombre"].ToString(),
                                PorcentajeDescuento = Convert.ToDecimal(dr["PorcentajeDescuento"]),

                                DescripcionItem = dr["DescripcionItem"] == DBNull.Value ? "" : dr["DescripcionItem"].ToString(),

                                Cantidad = Convert.ToDecimal(dr["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),

                                SubtotalLinea = Convert.ToDecimal(dr["SubtotalLinea"]),
                                TotalLinea = Convert.ToDecimal(dr["TotalLinea"]),

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
                lista = new List<DetalleFactura>();
            }

            return lista;
        }

        public int Registrar(DetalleFactura obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdFactura", obj.IdFactura);
                    cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("@IdDescuento", obj.IdDescuento == null ? (object)DBNull.Value : obj.IdDescuento);
                    cmd.Parameters.AddWithValue("@DescripcionItem", string.IsNullOrWhiteSpace(obj.DescripcionItem) ? (object)DBNull.Value : obj.DescripcionItem);
                    cmd.Parameters.AddWithValue("@Cantidad", obj.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", obj.PrecioUnitario);

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

        public bool Editar(DetalleFactura obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDetalleFactura", obj.IdDetalleFactura);
                    cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("@IdDescuento", obj.IdDescuento == null ? (object)DBNull.Value : obj.IdDescuento);
                    cmd.Parameters.AddWithValue("@DescripcionItem", string.IsNullOrWhiteSpace(obj.DescripcionItem) ? (object)DBNull.Value : obj.DescripcionItem);
                    cmd.Parameters.AddWithValue("@Cantidad", obj.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", obj.PrecioUnitario);
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

        public bool Inactivar(int idDetalleFactura, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDetalleFactura", idDetalleFactura);

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