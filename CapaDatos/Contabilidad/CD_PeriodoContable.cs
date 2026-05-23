using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_PeriodoContable
    {
        public List<PeriodoContable> Listar()
        {
            List<PeriodoContable> lista = new List<PeriodoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_PeriodoContable_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PeriodoContable()
                            {
                                IdPeriodoContable = Convert.ToInt32(dr["IdPeriodoContable"]),
                                Anio = Convert.ToInt32(dr["Anio"]),
                                Mes = Convert.ToInt32(dr["Mes"]),
                                FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                                FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                                Estado = dr["Estado"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PeriodoContable>();
            }

            return lista;
        }

        public List<PeriodoContable> ListarActivos()
        {
            List<PeriodoContable> lista = new List<PeriodoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_PeriodoContable_ListarActivos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PeriodoContable()
                            {
                                IdPeriodoContable = Convert.ToInt32(dr["IdPeriodoContable"]),
                                Anio = Convert.ToInt32(dr["Anio"]),
                                Mes = Convert.ToInt32(dr["Mes"]),
                                FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                                FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                                Estado = dr["Estado"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PeriodoContable>();
            }

            return lista;
        }

        public List<PeriodoContable> ListarAbierto()
        {
            List<PeriodoContable> lista = new List<PeriodoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_PeriodoContable_ListarAbierto", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PeriodoContable()
                            {
                                IdPeriodoContable = Convert.ToInt32(dr["IdPeriodoContable"]),
                                Anio = Convert.ToInt32(dr["Anio"]),
                                Mes = Convert.ToInt32(dr["Mes"]),
                                FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                                FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                                Estado = dr["Estado"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<PeriodoContable>();
            }

            return lista;
        }

        public int Registrar(PeriodoContable obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_PeriodoContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
                    cmd.Parameters.AddWithValue("@FechaInicio", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", obj.FechaFin);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado);

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

        public bool Editar(PeriodoContable obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_PeriodoContable_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPeriodoContable", obj.IdPeriodoContable);
                    cmd.Parameters.AddWithValue("@Anio", obj.Anio);
                    cmd.Parameters.AddWithValue("@Mes", obj.Mes);
                    cmd.Parameters.AddWithValue("@FechaInicio", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", obj.FechaFin);
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

        public bool Cerrar(int idPeriodoContable, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_PeriodoContable_Cerrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPeriodoContable", idPeriodoContable);

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

        public bool Inactivar(int idPeriodoContable, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_PeriodoContable_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPeriodoContable", idPeriodoContable);

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