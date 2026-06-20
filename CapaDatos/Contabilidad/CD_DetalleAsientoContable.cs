using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_DetalleAsientoContable
    {
        public List<DetalleAsientoContable> Listar()
        {
            List<DetalleAsientoContable> lista = new List<DetalleAsientoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearDetalle(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DetalleAsientoContable>();
            }

            return lista;
        }

        public List<DetalleAsientoContable> ListarPorAsiento(string numeroAsiento)
        {
            List<DetalleAsientoContable> lista = new List<DetalleAsientoContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_ListarPorAsiento", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", numeroAsiento ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearDetalle(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DetalleAsientoContable>();
            }

            return lista;
        }

        public DetalleAsientoContable Obtener(string numeroAsiento, int numeroLinea)
        {
            DetalleAsientoContable obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", numeroAsiento ?? "");
                    cmd.Parameters.AddWithValue("@NumeroLinea", numeroLinea);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearDetalle(dr);
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

        public int Registrar(DetalleAsientoContable obj, out string Mensaje, out int NumeroLineaGenerada)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroLineaGenerada = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", obj.NumeroAsiento ?? "");
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@Debe", obj.Debe);
                    cmd.Parameters.AddWithValue("@Haber", obj.Haber);
                    cmd.Parameters.AddWithValue("@DescripcionLinea", obj.DescripcionLinea ?? "");

                    cmd.Parameters.Add("@NumeroLineaGenerada", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroLineaGenerada = Convert.ToInt32(cmd.Parameters["@NumeroLineaGenerada"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroLineaGenerada = 0;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(DetalleAsientoContable obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", obj.NumeroAsiento ?? "");
                    cmd.Parameters.AddWithValue("@NumeroLinea", obj.NumeroLinea);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@Debe", obj.Debe);
                    cmd.Parameters.AddWithValue("@Haber", obj.Haber);
                    cmd.Parameters.AddWithValue("@DescripcionLinea", obj.DescripcionLinea ?? "");
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

        public bool Inactivar(string numeroAsiento, int numeroLinea, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", numeroAsiento ?? "");
                    cmd.Parameters.AddWithValue("@NumeroLinea", numeroLinea);

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

        public bool InactivarPorAsiento(string numeroAsiento, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_DetalleAsientoContable_InactivarPorAsiento", oconexion);
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

        private DetalleAsientoContable MapearDetalle(SqlDataReader dr)
        {
            return new DetalleAsientoContable()
            {
                NumeroAsiento = dr["NumeroAsiento"].ToString(),
                NumeroLinea = Convert.ToInt32(dr["NumeroLinea"]),

                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                NombreCuenta = dr["NombreCuenta"].ToString(),

                IdTipoCuentaContable = Convert.ToInt32(dr["IdTipoCuentaContable"]),
                TipoCuentaContableNombre = dr["TipoCuentaContableNombre"].ToString(),

                IdNaturalezaCuentaContable = Convert.ToInt32(dr["IdNaturalezaCuentaContable"]),
                NaturalezaCuentaContableNombre = dr["NaturalezaCuentaContableNombre"].ToString(),

                Debe = Convert.ToDecimal(dr["Debe"]),
                Haber = Convert.ToDecimal(dr["Haber"]),

                DescripcionLinea = dr["DescripcionLinea"].ToString(),

                Activo = Convert.ToBoolean(dr["Activo"])
            };
        }
    }
}