using CapaEntidad.Presupuesto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Presupuesto
{
    public class CD_DetallePresupuestoMensual
    {
        public List<DetallePresupuestoMensual> ListarPorPresupuesto(int anio, int mes)
        {
            List<DetallePresupuestoMensual> lista = new List<DetallePresupuestoMensual>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_ListarPorPresupuesto", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearDetallePresupuestoMensual(dr));
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

        public DetallePresupuestoMensual Obtener(int anio, int mes, string codigoCuenta, string tipoMovimiento)
        {
            DetallePresupuestoMensual obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", codigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@TipoMovimiento", tipoMovimiento ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearDetallePresupuestoMensual(dr);
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

                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@TipoMovimiento", obj.TipoMovimiento ?? "");
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

                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@TipoMovimiento", obj.TipoMovimiento ?? "");
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

        public bool Inactivar(int anio, int mes, string codigoCuenta, string tipoMovimiento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", codigoCuenta ?? "");
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

        public bool Activar(int anio, int mes, string codigoCuenta, string tipoMovimiento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Presupuesto.sp_DetallePresupuestoMensual_Activar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", codigoCuenta ?? "");
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

        private DetallePresupuestoMensual MapearDetallePresupuestoMensual(SqlDataReader dr)
        {
            DetallePresupuestoMensual obj = new DetallePresupuestoMensual()
            {
                Anio = Convert.ToInt32(dr["Anio"]),
                Mes = Convert.ToInt32(dr["Mes"]),

                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                NombreCuenta = dr["NombreCuenta"].ToString(),

                TipoMovimiento = dr["TipoMovimiento"].ToString(),

                MontoPresupuestado = Convert.ToDecimal(dr["MontoPresupuestado"]),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"]),

                MesNombre = dr["MesNombre"].ToString()
            };

            return obj;
        }
    }
}