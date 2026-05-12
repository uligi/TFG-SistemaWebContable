using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_CuentaContable
    {
        public List<CuentaContable> Listar()
        {
            List<CuentaContable> lista = new List<CuentaContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CuentaContable()
                            {
                                IdCuentaContable = Convert.ToInt32(dr["IdCuentaContable"]),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),

                                IdTipoCuentaContable = Convert.ToInt32(dr["IdTipoCuentaContable"]),
                                TipoCuentaContableNombre = dr["TipoCuentaContableNombre"].ToString(),

                                IdNaturalezaCuentaContable = Convert.ToInt32(dr["IdNaturalezaCuentaContable"]),
                                NaturalezaCuentaContableNombre = dr["NaturalezaCuentaContableNombre"].ToString(),

                                AceptaMovimientos = Convert.ToBoolean(dr["AceptaMovimientos"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),

                                IdCuentaPadre = dr["IdCuentaPadre"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdCuentaPadre"]),
                                CodigoCuentaPadre = dr["CodigoCuentaPadre"] == DBNull.Value ? "" : dr["CodigoCuentaPadre"].ToString(),
                                NombreCuentaPadre = dr["NombreCuentaPadre"] == DBNull.Value ? "" : dr["NombreCuentaPadre"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaContable>();
            }

            return lista;
        }

        public List<CuentaContable> ListarActivas()
        {
            List<CuentaContable> lista = new List<CuentaContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_ListarActivas", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CuentaContable()
                            {
                                IdCuentaContable = Convert.ToInt32(dr["IdCuentaContable"]),
                                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"].ToString(),

                                IdTipoCuentaContable = Convert.ToInt32(dr["IdTipoCuentaContable"]),
                                TipoCuentaContableNombre = dr["TipoCuentaContableNombre"].ToString(),

                                IdNaturalezaCuentaContable = Convert.ToInt32(dr["IdNaturalezaCuentaContable"]),
                                NaturalezaCuentaContableNombre = dr["NaturalezaCuentaContableNombre"].ToString(),

                                AceptaMovimientos = Convert.ToBoolean(dr["AceptaMovimientos"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),

                                IdCuentaPadre = dr["IdCuentaPadre"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdCuentaPadre"]),
                                CodigoCuentaPadre = dr["CodigoCuentaPadre"] == DBNull.Value ? "" : dr["CodigoCuentaPadre"].ToString(),
                                NombreCuentaPadre = dr["NombreCuentaPadre"] == DBNull.Value ? "" : dr["NombreCuentaPadre"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaContable>();
            }

            return lista;
        }

        public int Registrar(CuentaContable obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta);
                    cmd.Parameters.AddWithValue("@NombreCuenta", obj.NombreCuenta);
                    cmd.Parameters.AddWithValue("@IdTipoCuentaContable", obj.IdTipoCuentaContable);
                    cmd.Parameters.AddWithValue("@IdNaturalezaCuentaContable", obj.IdNaturalezaCuentaContable);
                    cmd.Parameters.AddWithValue("@AceptaMovimientos", obj.AceptaMovimientos);
                    cmd.Parameters.AddWithValue("@IdCuentaPadre", obj.IdCuentaPadre.HasValue ? (object)obj.IdCuentaPadre.Value : DBNull.Value);

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

        public bool Editar(CuentaContable obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta);
                    cmd.Parameters.AddWithValue("@NombreCuenta", obj.NombreCuenta);
                    cmd.Parameters.AddWithValue("@IdTipoCuentaContable", obj.IdTipoCuentaContable);
                    cmd.Parameters.AddWithValue("@IdNaturalezaCuentaContable", obj.IdNaturalezaCuentaContable);
                    cmd.Parameters.AddWithValue("@AceptaMovimientos", obj.AceptaMovimientos);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("@IdCuentaPadre", obj.IdCuentaPadre.HasValue ? (object)obj.IdCuentaPadre.Value : DBNull.Value);

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

        public bool Inactivar(int idCuentaContable, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaContable", idCuentaContable);

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

        public string GenerarCodigo(int idCuentaPadre, out string Mensaje)
        {
            string codigoGenerado = string.Empty;
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_GenerarCodigo", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPadre", idCuentaPadre);

                    cmd.Parameters.Add("@CodigoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                    if (resultado)
                    {
                        codigoGenerado = cmd.Parameters["@CodigoGenerado"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                codigoGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return codigoGenerado;
        }
    }
}