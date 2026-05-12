using CapaEntidad.Inventario;
using CapaEntidad.Catalogos;
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
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                                TipoProductoNombre = dr["TipoProductoNombre"].ToString(),
                                Descripcion = dr["Descripcion"] == DBNull.Value ? "" : dr["Descripcion"].ToString(),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                StockActual = Convert.ToDecimal(dr["StockActual"]),
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
                lista = new List<Producto>();
            }

            return lista;
        }

        public List<TipoProducto> ListarTipoProductosActivos()
        {
            List<TipoProducto> lista = new List<TipoProducto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_TipoProducto_ListarActivos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new TipoProducto()
                            {
                                IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                                TipoProductoNombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"] == DBNull.Value ? "" : dr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<TipoProducto>();
            }

            return lista;
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@NombreProducto", obj.NombreProducto);
                    cmd.Parameters.AddWithValue("@IdTipoProducto", obj.IdTipoProducto);
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(obj.Descripcion) ? (object)DBNull.Value : obj.Descripcion);
                    cmd.Parameters.AddWithValue("@PrecioVenta", obj.PrecioVenta);
                    cmd.Parameters.AddWithValue("@StockActual", obj.StockActual);

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

                    cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("@CodigoProducto", obj.CodigoProducto);
                    cmd.Parameters.AddWithValue("@NombreProducto", obj.NombreProducto);
                    cmd.Parameters.AddWithValue("@IdTipoProducto", obj.IdTipoProducto);
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(obj.Descripcion) ? (object)DBNull.Value : obj.Descripcion);
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

        public bool Inactivar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Inventario.sp_Producto_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdProducto", id);

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