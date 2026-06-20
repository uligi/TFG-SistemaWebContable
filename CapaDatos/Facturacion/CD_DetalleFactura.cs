using CapaEntidad.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Facturacion
{
    public class CD_DetalleFactura
    {
        public List<DetalleFactura> Listar()
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearDetalleFactura(dr));
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

        public List<DetalleFactura> ListarPorFactura(string numeroFactura)
        {
            List<DetalleFactura> lista = new List<DetalleFactura>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_ListarPorFactura", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearDetalleFactura(dr));
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

        public DetalleFactura Obtener(string numeroFactura, string codigoProducto)
        {
            DetalleFactura obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@CodigoProducto", codigoProducto ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearDetalleFactura(dr);
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

                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@CodigoProducto", obj.CodigoProducto ?? "");
                    cmd.Parameters.AddWithValue("@IdImpuesto", obj.IdImpuesto);
                    cmd.Parameters.AddWithValue("@IdDescuento", obj.IdDescuento);
                    cmd.Parameters.AddWithValue("@DescripcionItem", obj.DescripcionItem ?? "");
                    cmd.Parameters.AddWithValue("@Cantidad", obj.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", obj.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@PorcentajeImpuesto", obj.PorcentajeImpuesto);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", obj.PorcentajeDescuento);

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

                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@CodigoProducto", obj.CodigoProducto ?? "");
                    cmd.Parameters.AddWithValue("@IdImpuesto", obj.IdImpuesto);
                    cmd.Parameters.AddWithValue("@IdDescuento", obj.IdDescuento);
                    cmd.Parameters.AddWithValue("@DescripcionItem", obj.DescripcionItem ?? "");
                    cmd.Parameters.AddWithValue("@Cantidad", obj.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", obj.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@PorcentajeImpuesto", obj.PorcentajeImpuesto);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", obj.PorcentajeDescuento);
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

        public bool Inactivar(string numeroFactura, string codigoProducto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_DetalleFactura_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@CodigoProducto", codigoProducto ?? "");

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

        private DetalleFactura MapearDetalleFactura(SqlDataReader dr)
        {
            return new DetalleFactura()
            {
                NumeroFactura = dr["NumeroFactura"].ToString(),

                CodigoProducto = dr["CodigoProducto"].ToString(),
                NombreProducto = dr["NombreProducto"].ToString(),

                IdImpuesto = Convert.ToInt32(dr["IdImpuesto"]),
                ImpuestoNombre = dr["ImpuestoNombre"].ToString(),

                IdDescuento = Convert.ToInt32(dr["IdDescuento"]),
                DescuentoNombre = dr["DescuentoNombre"].ToString(),

                DescripcionItem = dr["DescripcionItem"].ToString(),

                Cantidad = Convert.ToDecimal(dr["Cantidad"]),
                PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),

                PorcentajeImpuesto = Convert.ToDecimal(dr["PorcentajeImpuesto"]),
                PorcentajeDescuento = Convert.ToDecimal(dr["PorcentajeDescuento"]),

                SubtotalLinea = Convert.ToDecimal(dr["SubtotalLinea"]),
                TotalLinea = Convert.ToDecimal(dr["TotalLinea"]),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };
        }
    }
}