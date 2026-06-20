using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_AsientoContable
    {
        public List<AsientoContable> Listar()
        {
            List<AsientoContable> lista = new List<AsientoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearAsiento(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<AsientoContable>();
            }

            return lista;
        }

        public List<AsientoContable> ListarPorPeriodo(int anio, int mes)
        {
            List<AsientoContable> lista = new List<AsientoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_ListarPorPeriodo", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cmd.Parameters.AddWithValue("@Mes", mes);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearAsiento(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<AsientoContable>();
            }

            return lista;
        }

        public AsientoContable Obtener(string numeroAsiento)
        {
            AsientoContable obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", numeroAsiento ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearAsiento(dr);
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

        public int Registrar(AsientoContable obj, out string Mensaje, out string NumeroAsientoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroAsientoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
                    cmd.Parameters.AddWithValue("@FechaAsiento", obj.FechaAsiento);
                    cmd.Parameters.AddWithValue("@TipoAsiento", obj.TipoAsiento ?? "");
                    cmd.Parameters.AddWithValue("@Concepto", obj.Concepto ?? "");

                    cmd.Parameters.Add("@NumeroAsientoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroAsientoGenerado = cmd.Parameters["@NumeroAsientoGenerado"].Value.ToString();
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroAsientoGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(AsientoContable obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", obj.NumeroAsiento ?? "");
                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
                    cmd.Parameters.AddWithValue("@FechaAsiento", obj.FechaAsiento);
                    cmd.Parameters.AddWithValue("@TipoAsiento", obj.TipoAsiento ?? "");
                    cmd.Parameters.AddWithValue("@Concepto", obj.Concepto ?? "");
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

        public bool ValidarCuadre(string numeroAsiento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_ValidarCuadre", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", numeroAsiento ?? "");

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

        public bool RecalcularTotales(string numeroAsiento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_RecalcularTotales", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", numeroAsiento ?? "");

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = true;
                    Mensaje = "Totales recalculados correctamente.";
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Inactivar(string numeroAsiento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_AsientoContable_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", numeroAsiento ?? "");

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

        private AsientoContable MapearAsiento(SqlDataReader dr)
        {
            return new AsientoContable()
            {
                NumeroAsiento = dr["NumeroAsiento"].ToString(),

                Anio = Convert.ToInt32(dr["Anio"]),
                Mes = Convert.ToInt32(dr["Mes"]),

                FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                EstadoPeriodo = dr["EstadoPeriodo"].ToString(),

                FechaAsiento = Convert.ToDateTime(dr["FechaAsiento"]),
                TipoAsiento = dr["TipoAsiento"].ToString(),
                Concepto = dr["Concepto"].ToString(),

                TotalDebe = Convert.ToDecimal(dr["TotalDebe"]),
                TotalHaber = Convert.ToDecimal(dr["TotalHaber"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };
        }
    }
}