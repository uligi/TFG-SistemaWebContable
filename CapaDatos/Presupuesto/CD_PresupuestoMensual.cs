using CapaEntidad.Presupuesto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Presupuesto
{
    public class CD_PresupuestoMensual
    {
        public List<PresupuestoMensual> Listar()
        {
            List<PresupuestoMensual> lista = new List<PresupuestoMensual>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearPresupuestoMensual(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PresupuestoMensual>();
            }

            return lista;
        }

        public PresupuestoMensual Obtener(int anio, int mes)
        {
            PresupuestoMensual obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearPresupuestoMensual(dr);
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

        public int Registrar(PresupuestoMensual obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado ?? "");

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

        public bool Editar(PresupuestoMensual obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
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

        public bool Inactivar(int anio, int mes, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

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

        public bool Cerrar(int anio, int mes, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Cerrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

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

        public bool Reabrir(int anio, int mes, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Reabrir", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

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

        public List<ResumenPresupuestoMensual> ResumenVsReal(int anio, int mes)
        {
            List<ResumenPresupuestoMensual> lista = new List<ResumenPresupuestoMensual>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_ResumenVsReal", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ResumenPresupuestoMensual()
                            {
                                TipoMovimiento = dr["TipoMovimiento"].ToString(),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),

                                MontoPresupuestado = Convert.ToDecimal(dr["MontoPresupuestado"]),
                                MontoReal = Convert.ToDecimal(dr["MontoReal"]),
                                Diferencia = Convert.ToDecimal(dr["Diferencia"]),
                                PorcentajeEjecucion = Convert.ToDecimal(dr["PorcentajeEjecucion"]),

                                EstadoEjecucion = dr["EstadoEjecucion"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ResumenPresupuestoMensual>();
            }

            return lista;
        }

        public ResumenPresupuestoMensual ResumenGeneral(int anio, int mes)
        {
            ResumenPresupuestoMensual obj = new ResumenPresupuestoMensual();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_ResumenGeneral", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj.TotalIngresoPresupuestado = dr["TotalIngresoPresupuestado"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalIngresoPresupuestado"]);
                            obj.TotalIngresoReal = dr["TotalIngresoReal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalIngresoReal"]);

                            obj.TotalGastoPresupuestado = dr["TotalGastoPresupuestado"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalGastoPresupuestado"]);
                            obj.TotalGastoReal = dr["TotalGastoReal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalGastoReal"]);

                            obj.UtilidadPresupuestada = dr["UtilidadPresupuestada"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["UtilidadPresupuestada"]);
                            obj.UtilidadReal = dr["UtilidadReal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["UtilidadReal"]);
                        }
                    }
                }
            }
            catch
            {
                obj = new ResumenPresupuestoMensual();
            }

            return obj;
        }

        private PresupuestoMensual MapearPresupuestoMensual(SqlDataReader dr)
        {
            PresupuestoMensual obj = new PresupuestoMensual()
            {
                Anio = Convert.ToInt32(dr["Anio"]),
                Mes = Convert.ToInt32(dr["Mes"]),

                Estado = dr["Estado"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"]),

                MesNombre = dr["MesNombre"].ToString()
            };

            return obj;
        }
    }
}