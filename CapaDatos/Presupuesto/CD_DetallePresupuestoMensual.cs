using CapaEntidad.Presupuesto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Presupuesto
{
    public class CD_DetallePresupuestoMensual
    {
        public List<DetallePresupuestoMensual> ListarPorPresupuesto(int idPresupuestoMensual)
        {
            List<DetallePresupuestoMensual> lista = new List<DetallePresupuestoMensual>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_ListarPorPresupuesto", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPresupuestoMensual", idPresupuestoMensual);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetallePresupuestoMensual()
                            {
                                IdDetallePresupuestoMensual = Convert.ToInt32(dr["IdDetallePresupuestoMensual"]),
                                IdPresupuestoMensual = Convert.ToInt32(dr["IdPresupuestoMensual"]),
                                IdCuentaContable = Convert.ToInt32(dr["IdCuentaContable"]),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),
                                TipoMovimiento = dr["TipoMovimiento"].ToString(),
                                MontoPresupuestado = Convert.ToDecimal(dr["MontoPresupuestado"]),
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
                lista = new List<DetallePresupuestoMensual>();
            }

            return lista;
        }

        public int Registrar(DetallePresupuestoMensual obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPresupuestoMensual", obj.IdPresupuestoMensual);
                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", obj.TipoMovimiento);
                    cmd.Parameters.AddWithValue("@MontoPresupuestado", obj.MontoPresupuestado);

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

        public bool Editar(DetallePresupuestoMensual obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDetallePresupuestoMensual", obj.IdDetallePresupuestoMensual);
                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", obj.TipoMovimiento);
                    cmd.Parameters.AddWithValue("@MontoPresupuestado", obj.MontoPresupuestado);
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

        public bool Inactivar(int idDetallePresupuestoMensual, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDetallePresupuestoMensual", idDetallePresupuestoMensual);

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