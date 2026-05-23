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
                            lista.Add(new PresupuestoMensual()
                            {
                                IdPresupuestoMensual = Convert.ToInt32(dr["IdPresupuestoMensual"]),
                                Anio = Convert.ToInt32(dr["Anio"]),
                                Mes = Convert.ToInt32(dr["Mes"]),
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
                lista = new List<PresupuestoMensual>();
            }

            return lista;
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

                    cmd.Parameters.AddWithValue("@IdPresupuestoMensual", obj.IdPresupuestoMensual);
                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
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

        public bool Inactivar(int idPresupuestoMensual, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPresupuestoMensual", idPresupuestoMensual);

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

        public List<ResumenPresupuestoMensual> ResumenVsReal(int idPresupuestoMensual)
        {
            List<ResumenPresupuestoMensual> lista = new List<ResumenPresupuestoMensual>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_PresupuestoMensual_ResumenVsReal", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPresupuestoMensual", idPresupuestoMensual);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ResumenPresupuestoMensual()
                            {
                                TipoMovimiento = dr["TipoMovimiento"].ToString(),
                                IdCuentaContable = Convert.ToInt32(dr["IdCuentaContable"]),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),
                                MontoPresupuestado = Convert.ToDecimal(dr["MontoPresupuestado"]),
                                MontoReal = Convert.ToDecimal(dr["MontoReal"]),
                                Diferencia = Convert.ToDecimal(dr["Diferencia"])
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
    }
}