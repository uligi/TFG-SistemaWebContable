using CapaEntidad.Inventario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Inventario
{
    public class CD_Producto
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearProducto(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }

            return lista;
        }

        public List<Producto> ListarActivos()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_ListarActivos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearProducto(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }

            return lista;
        }

        public Producto Obtener(string codigoProducto)
        {
            Producto obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoProducto", codigoProducto ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearProducto(dr);
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

        public int Registrar(Producto obj, out string Mensaje, out string CodigoProductoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            CodigoProductoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NombreProducto", obj.NombreProducto ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoProducto", obj.IdTipoProducto);
                    cmd.Parameters.AddWithValue("@IdImpuesto", obj.IdImpuesto);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@PrecioVenta", obj.PrecioVenta);
                    cmd.Parameters.AddWithValue("@StockActual", obj.StockActual);

                    cmd.Parameters.Add("@CodigoProductoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    CodigoProductoGenerado = cmd.Parameters["@CodigoProductoGenerado"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                Mensaje = ex.Message;
                CodigoProductoGenerado = string.Empty;
            }

            return resultado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoProducto", obj.CodigoProducto ?? "");
                    cmd.Parameters.AddWithValue("@NombreProducto", obj.NombreProducto ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoProducto", obj.IdTipoProducto);
                    cmd.Parameters.AddWithValue("@IdImpuesto", obj.IdImpuesto);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@PrecioVenta", obj.PrecioVenta);
                    cmd.Parameters.AddWithValue("@StockActual", obj.StockActual);
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

        public bool AjustarStock(string codigoProducto, decimal cantidad, string tipoMovimiento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_AjustarStock", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoProducto", codigoProducto ?? "");
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", tipoMovimiento ?? "");

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

        public bool Inactivar(string codigoProducto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

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

        private Producto MapearProducto(SqlDataReader dr)
        {
            return new Producto()
            {
                CodigoProducto = dr["CodigoProducto"].ToString(),
                NombreProducto = dr["NombreProducto"].ToString(),

                IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                TipoProductoNombre = dr["TipoProductoNombre"].ToString(),

                IdImpuesto = Convert.ToInt32(dr["IdImpuesto"]),
                ImpuestoNombre = dr["ImpuestoNombre"].ToString(),
                PorcentajeImpuesto = Convert.ToDecimal(dr["PorcentajeImpuesto"]),

                Descripcion = dr["Descripcion"].ToString(),
                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                StockActual = Convert.ToDecimal(dr["StockActual"]),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };
        }
    }
}